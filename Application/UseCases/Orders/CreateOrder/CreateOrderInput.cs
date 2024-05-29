using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.CreateOrder
{
    public class CreateOrderInput : IRequest<Order>
    {
        public Guid CLientId { get; set; }
        public Guid SellerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
    }
}
