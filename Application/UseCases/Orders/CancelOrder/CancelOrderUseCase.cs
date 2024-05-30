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

        public async Task<CancelOrderOutput> Handle(CancelOrderInput input, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"User {input.UserId} requested order cancellation.");
            
            var order = await _orderRepository.GetOrderByIdAsync(input.OrderId).ConfigureAwait(false);
            
            if (order.Status == OrderStatus.Pending)
            {
                order.Status = OrderStatus.Cancelled;
                order.OrderStatusDescription = OrderStatus.Cancelled.ToString();
                order.UpdatedAt = DateTime.Now; 
                
                var result = await _orderRepository.UpdateOrderAsync(order).ConfigureAwait(false);

                return new CancelOrderOutput()
                {
                    OrderId = result.OrderId,
                    Status = OrderStatus.Cancelled.ToString(),
                    Message = "Customer Request to cancell order with success.",
                    CancelationDate = result.UpdatedAt,
                    TotalAmount = result.TotalPrice
                };
            }
            return new CancelOrderOutput()
            {
                OrderId = order.OrderId,
                Status = order.OrderStatusDescription,
                Message = "You can only cancel Pending orders.",
                TotalAmount = order.TotalPrice
            };

        }
    }
}
