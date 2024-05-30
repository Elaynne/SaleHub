using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.GetOrder
{
    public class GetOrderInput : IRequest<Order>
    {
        public Guid UserId { get; set; }
        public UserRole UserRole { get; set; }
        public Guid OrderId { get; set; }
    }
}
