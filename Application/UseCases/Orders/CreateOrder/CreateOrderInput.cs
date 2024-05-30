using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.CreateOrder
{
    public class CreateOrderInput : IRequest<Order>
    {
        public Guid UserId { get; set; }
        public Guid ClientId { get; set; }
        public Guid SellerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
