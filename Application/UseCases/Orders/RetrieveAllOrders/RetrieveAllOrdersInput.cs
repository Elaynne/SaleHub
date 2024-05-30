using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.RetrieveAllOrders
{
    public class RetrieveAllOrdersInput : IRequest<IEnumerable<Order>>
    {
        public Guid UserId { get; set; }
        public UserRole UserRole { get; set; }
    }
}
