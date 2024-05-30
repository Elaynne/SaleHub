using Domain.Models;
using Domain.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusUseCase : IUpdateOrderStatusUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<UpdateOrderStatusUseCase> _logger;
        public UpdateOrderStatusUseCase(IOrderRepository orderRepository,
            ILogger<UpdateOrderStatusUseCase> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Order> Handle(UpdateOrderStatusInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"User {input.UserId} requested order status update to {input.Status}.");
            
            var order = await _orderRepository.GetOrderByIdAsync(input.OrderId).ConfigureAwait(false);
            order.Status = input.Status;

            return await _orderRepository.UpdateOrderAsync(order).ConfigureAwait(false);

        }
    }
}
