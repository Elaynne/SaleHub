using MediatR;
using Domain.Models;

namespace Application.UseCases.Orders.CreateOrder
{
    public interface ICreateOrder : IRequestHandler<CreateOrderInput, Order>
    {
    }
}
