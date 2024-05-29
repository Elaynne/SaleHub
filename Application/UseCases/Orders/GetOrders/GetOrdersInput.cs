using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.GetOrders
{
    public class GetOrdersInput : IRequest<IEnumerable<Order>>
    {
        public Guid UserId { get; set; }
        public UserRole UserRole { get; set; }
    }
}
