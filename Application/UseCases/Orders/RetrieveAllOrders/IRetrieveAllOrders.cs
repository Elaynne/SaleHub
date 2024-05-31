using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.RetrieveAllOrders
{
    public interface IRetrieveAllOrders : IRequestHandler<RetrieveAllOrdersInput, IEnumerable<Order>>
    {
    }
}
