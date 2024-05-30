using Application.UseCases.Books.GetBook;
using Application.UseCases.Orders.CreateOrder;
using AutoMapper;
using Domain.Cache;
using Domain.Models;
using Domain.Repository.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;


namespace UnitTests.Application.UseCases.Orders
{
    public class CreateOrderUseCaseTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CreateOrderUseCase> _logger;
        private readonly IMapper _mapper;
        private readonly IGetBookUseCase _getBookUseCase;
        private readonly ICacheService<Book> _cacheService;
        private readonly CreateOrderUseCase _useCase;
        private readonly List<OrderItem> _orderItems;
        private readonly CreateOrderInput _request;
       
        public CreateOrderUseCaseTests()
        {
            _orderItems = new List<OrderItem>
            {
                new OrderItem { BookId = Guid.NewGuid(), Quantity = 2},
                new OrderItem { BookId = Guid.NewGuid(), Quantity = 3}
            };

            _request = new CreateOrderInput()
             {
                UserId = Guid.NewGuid(),
                OrderItems = _orderItems
            };

            _orderRepository = Substitute.For<IOrderRepository>();
            _logger = Substitute.For<ILogger<CreateOrderUseCase>>();
            _mapper = Substitute.For<IMapper>();
            _getBookUseCase = Substitute.For<IGetBookUseCase>();
            _cacheService = Substitute.For<ICacheService<Book>>();
            _useCase = new CreateOrderUseCase(
                _orderRepository,
                _logger,
                _mapper,
                _getBookUseCase,
                _cacheService
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateOrder_WhenAllItemsInStock()
        {
            var order = new Order { Id = Guid.NewGuid()};
            var books = _orderItems.Select(x => new Book { Id = x.BookId, Stock = 10 }).ToList();

            _mapper.Map<Order>(_request).Returns(order);
            foreach (var book in books)
            {
                _getBookUseCase.Handle(Arg.Is<GetBookInput>(x => x.BookId == book.Id), Arg.Any<CancellationToken>())
                    .Returns(book);
            }
            _cacheService.UpdateCacheAsync(
                    Arg.Any<List<Guid>>(),
                    Arg.Any<Func<Guid, Task<Book>>>(),
                    Arg.Any<Func<Task<Dictionary<Guid, Book>>>>(),
                    CacheKeys.BooksKey, "Book")
                .Returns(Task.FromResult(true));
            _orderRepository.AddOrderAsync(order).Returns(Task.FromResult(order));

            var result = await _useCase.Handle(_request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
            await _orderRepository.Received(1).AddOrderAsync(order);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenAnyItemOutOfStock()
        {
            var books = _orderItems.Select(x => new Book { Id = x.BookId, Stock = 1 }).ToList();

            foreach (var book in books)
            {
                _getBookUseCase.Handle(Arg.Is<GetBookInput>(x => x.BookId == book.Id), Arg.Any<CancellationToken>())
                    .Returns(book);
            }

            var result = await _useCase.Handle(_request, CancellationToken.None);

            Assert.Null(result);
            await _orderRepository.DidNotReceive().AddOrderAsync(Arg.Any<Order>());
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenAnyBookNotFound()
        {
            _getBookUseCase.Handle(Arg.Any<GetBookInput>(), Arg.Any<CancellationToken>())
                .Throws(new NotFoundException("Book not found"));

            var result = await _useCase.Handle(_request, CancellationToken.None);

            Assert.Null(result);
            await _orderRepository.DidNotReceive().AddOrderAsync(Arg.Any<Order>());
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCacheUpdateFails()
        {
            var order = new Order { Id = Guid.NewGuid(),};
            var books = _orderItems.Select(x => new Book { Id = x.BookId, Stock = 10 }).ToList();

            _mapper.Map<Order>(_request).Returns(order);
            foreach (var book in books)
            {
                _getBookUseCase.Handle(Arg.Is<GetBookInput>(i => i.BookId == book.Id), Arg.Any<CancellationToken>())
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