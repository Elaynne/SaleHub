using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.CancelOrder
{
    public class CancelOrderInput : IRequest<Order>
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
    }
}
