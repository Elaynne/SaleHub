using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.CreateOrder
{
    public class CreateOrderInput : IRequest<Order>
    {
        public Guid ClientId { get; set; }
        public Guid SellerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }

        public CreateOrderInput()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
