using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.RetrieveOrderById
{
    public interface IRetrieveOrderById : IRequestHandler<RetrieveOrderByIdInput, Order>
    {
    }
}
