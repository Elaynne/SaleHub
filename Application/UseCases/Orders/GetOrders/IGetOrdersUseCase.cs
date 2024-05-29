using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.GetOrders
{
    public interface IGetOrdersUseCase : IRequestHandler<GetOrdersInput, IEnumerable<Order>>
    {
    }
}
