using Application.UseCases.Books.RetrieveBookById;
using Application.UseCases.Orders.CreateOrder;
using Domain.Cache;
using Domain.Models;
using Domain.Repository.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;


namespace UnitTests.Application.UseCases.Orders
{
    public class CreateOrderTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CreateOrder> _logger;
        private readonly IRetrieveBookById _IRetrieveBookByIdUseCase;
        private readonly ICacheService<Book> _cacheService;
        private readonly CreateOrder _useCase;
        private readonly List<OrderItem> _orderItems;
        private readonly CreateOrderInput _request;
       
        public CreateOrderTests()
        {
            _orderItems = new List<OrderItem>
            {
                new OrderItem { BookId = Guid.NewGuid(), Quantity = 2},
                new OrderItem { BookId = Guid.NewGuid(), Quantity = 3}
            };

            _request = new CreateOrderInput()
             {
                ClientId = Guid.NewGuid(),
                SellerId = Guid.NewGuid(),
                OrderItems = _orderItems
            };

            _orderRepository = Substitute.For<IOrderRepository>();
            _logger = Substitute.For<ILogger<CreateOrder>>();
            _IRetrieveBookByIdUseCase = Substitute.For<IRetrieveBookById>();
            _cacheService = Substitute.For<ICacheService<Book>>();
            _useCase = new CreateOrder(
                _orderRepository,
                _logger,
                _IRetrieveBookByIdUseCase,
                _cacheService
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateOrder_WhenAllItemsInStock()
        {
            var order = new Order { OrderId = Guid.NewGuid() };
            var books = _orderItems.Select(x => new Book { Id = x.BookId, Stock = 10 }).ToList();

            foreach (var book in books)
            {
                _IRetrieveBookByIdUseCase.Handle(Arg.Is<RetrieveBookByIdInput>(x => x.BookId == book.Id), Arg.Any<CancellationToken>())
                    .Returns(book);
            }
            _cacheService.UpdateCacheAsync(
                    Arg.Any<List<Guid>>(),
                    Arg.Any<Func<Guid, Task<Book>>>(),
                    Arg.Any<Func<Task<Dictionary<Guid, Book>>>>(),
                    CacheKeys.BooksKey, "Book")
                .Returns(Task.FromResult(true));

            _orderRepository.AddOrderAsync(Arg.Any<Order>()).Returns(Task.FromResult(order));

            var result = await _useCase.Handle(_request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(order.OrderId, result.OrderId);
            await _orderRepository.Received(1).AddOrderAsync(Arg.Any<Order>());
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenAnyItemOutOfStock()
        {
            var books = _orderItems.Select(x => new Book { Id = x.BookId, Stock = 1 }).ToList();

            foreach (var book in books)
            {
                _IRetrieveBookByIdUseCase.Handle(Arg.Is<RetrieveBookByIdInput>(x => x.BookId == book.Id), Arg.Any<CancellationToken>())
                    .Returns(book);
            }

            var result = await _useCase.Handle(_request, CancellationToken.None);

            Assert.Null(result);
            await _orderRepository.DidNotReceive().AddOrderAsync(Arg.Any<Order>());
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenAnyBookNotFound()
        {
            _IRetrieveBookByIdUseCase.Handle(Arg.Any<RetrieveBookByIdInput>(), Arg.Any<CancellationToken>())
                .Throws(new NotFoundException("Book not found"));

            var result = await _useCase.Handle(_request, CancellationToken.None);

            Assert.Null(result);
            await _orderRepository.DidNotReceive().AddOrderAsync(Arg.Any<Order>());
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCacheUpdateFails()
        {
            var order = new Order { OrderId = Guid.NewGuid(),};
            var books = _orderItems.Select(x => new Book { Id = x.BookId, Stock = 10 }).ToList();

            foreach (var book in books)
            {
                _IRetrieveBookByIdUseCase.Handle(Arg.Is<RetrieveBookByIdInput>(i => i.BookId == book.Id), Arg.Any<CancellationToken>())
                    .Returns(book);
            }
            _cacheService.UpdateCacheAsync(
                    Arg.Any<List<Guid>>(),
                    Arg.Any<Func<Guid, Task<Book>>>(),
                    Arg.Any<Func<Task<Dictionary<Guid, Book>>>>(),
                    CacheKeys.BooksKey, "Book")
                .Returns(Task.FromResult(false));

            var result = await _useCase.Handle(_request, CancellationToken.None);

            Assert.Null(result);
            await _orderRepository.DidNotReceive().AddOrderAsync(Arg.Any<Order>());
        }
    }

}