using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusInput : IRequest<Order>
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public Guid UserId { get; set; }
    }
}
