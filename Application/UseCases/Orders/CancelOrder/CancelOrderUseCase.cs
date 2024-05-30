using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Orders.CancelOrder
{
    public class CancelOrderUseCase : ICanceloOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CancelOrderUseCase> _logger;
        public CancelOrderUseCase(IOrderRepository orderRepository,
            ILogger<CancelOrderUseCase> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Order> Handle(CancelOrderInput input, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"User {input.UserId} requested order cancellation.");
            
            var order = await _orderRepository.GetOrderByIdAsync(input.OrderId).ConfigureAwait(false);
            order.Status = OrderStatus.Canceled;

            return await _orderRepository.UpdateOrderAsync(order).ConfigureAwait(false);

        }
    }
}
