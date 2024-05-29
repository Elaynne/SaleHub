﻿using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Orders.GetOrders
{
    public class GetOrdersUseCase : IGetOrdersUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<GetOrdersUseCase> _logger;
        public GetOrdersUseCase(IOrderRepository orderRepository,
            ILogger<GetOrdersUseCase> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Order>?> Handle(GetOrdersInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting orders for user {UserId}", input.UserId);

            var orders = await _orderRepository.GetAllOrdersAsync();

            return input.UserRole switch
            {
                UserRole.Admin => orders,
                UserRole.Seller => orders.Where(x => x.SellerId == input.UserId).ToList(),
                UserRole.Client => orders.Where(x => x.ClientId == input.UserId).ToList(),
                _ => null
            };
        }
    }
}
