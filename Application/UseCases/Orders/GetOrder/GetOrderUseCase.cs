using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Orders.GetOrder
{
    public class GetOrderUseCase : IGetOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<GetOrderUseCase> _logger;
        public GetOrderUseCase(IOrderRepository orderRepository,
            ILogger<GetOrderUseCase> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Order?> Handle(GetOrderInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting orders for user {UserId}", input.UserId);

            var order = await _orderRepository.GetOrderByIdAsync(input.OrderId);

            return input.UserRole switch
            {
                UserRole.Admin => order,
                UserRole.Seller => (order.SellerId == input.UserId) ? order : null,
                UserRole.Client => (order.ClientId == input.UserId) ? order: null,
                _ => null
            };
        }
    }
}
