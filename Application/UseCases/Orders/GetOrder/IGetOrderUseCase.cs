using Application.UseCases.Orders.GetOrders;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.GetOrder
{
    public interface IGetOrderUseCase : IRequestHandler<GetOrderInput, Order>
    {
    }
}
