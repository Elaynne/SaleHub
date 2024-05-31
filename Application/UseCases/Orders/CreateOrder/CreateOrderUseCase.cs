﻿using Application.UseCases.Books.GetBook;
using AutoMapper;
using Domain.Cache;
using Domain.Models;
using Domain.Repository.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Orders.CreateOrder
{
    public class CreateOrderUseCase : ICreateOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CreateOrderUseCase> _logger;
        private readonly IMapper _mapper;
        private readonly IGetBookUseCase _getBookUseCase;
        private readonly ICacheService<Book> _cacheService;
        private readonly List<Book> _fetchedItems;
        private readonly Dictionary<Guid, Book> _fetchFinalList;

        public CreateOrderUseCase(IOrderRepository orderRepository,
            ILogger<CreateOrderUseCase> logger,
            IMapper mapper,
            IGetBookUseCase getBookUseCase,
            ICacheService<Book> cacheService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _mapper = mapper;
            _getBookUseCase = getBookUseCase;
            _cacheService = cacheService;
            _fetchedItems = new List<Book>();
            _fetchFinalList = new Dictionary<Guid, Book>();
        }

        public async Task<Order> Handle(CreateOrderInput request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {UserId} creating order.", request.UserId);

            var order = _mapper.Map<Order>(request);

            var totalPrice = 0m;

            foreach (var item in request.OrderItems)
            {
                try {
                    var responseBook = await _getBookUseCase.Handle(new GetBookInput()
                    {
                        UserId = request.UserId,
                        BookId = item.BookId
                    }, cancellationToken).ConfigureAwait(false);

                    if (responseBook == null || responseBook.Stock < item.Quantity)
                    {
                        _logger.LogInformation("Some of the items on your order is out of stock");
                        return null; 
                    }
                    var partialPrice = responseBook.Price * item.Quantity;
                    totalPrice += partialPrice;

                    responseBook.Stock -= item.Quantity;
                    _fetchedItems.Add(responseBook);
                    _fetchFinalList.Add(responseBook.Id, responseBook);

                }
                catch (NotFoundException ex)
                {
                    _logger.LogInformation("Some of the items on your order is out of stock");
                    return null;
                }
            }

            var bookIds = request.OrderItems.Select(x => x.BookId).ToList();

            bool result = await _cacheService.UpdateCacheAsync<Book>(
                bookIds,
                async (id) => await FetchItemByIdAsync(id),
                async () => await FetchFinalListAsync(),
                CacheKeys.BooksKey, "Book");

            if (result)
            {
                _logger.LogInformation("Cache updated successfully.");

                order.TotalPrice = totalPrice;
                return await _orderRepository.AddOrderAsync(order);
            }
            else
            {
                _logger.LogInformation("Cache update failed and rollback completed.");
                return null;
            }
        }

        private async Task<Dictionary<Guid, Book>> FetchFinalListAsync() => _fetchFinalList;

        private async Task<Book> FetchItemByIdAsync(Guid id) => _fetchedItems.FirstOrDefault(x => x.Id == id);
        
    }
}