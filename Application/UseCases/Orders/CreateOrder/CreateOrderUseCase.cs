using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Orders.CreateOrder
{
    public class CreateOrderUseCase : ICreateOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> Handle(CreateOrderInput request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                ClientId = request.CLientId,
                SellerId = request.SellerId,
                OrderItems = request.OrderItems,
                OrderDate = request.OrderDate,
                TotalPrice = request.TotalPrice,
                Status = request.Status
            };

            return await _orderRepository.AddOrderAsync(order);
        }
    }
}
