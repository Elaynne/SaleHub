using MediatR;
using Domain.Models;

namespace Application.UseCases.Orders.CreateOrder
{
    public interface ICreateOrderUseCase : IRequestHandler<CreateOrderInput, Order>
    {
    }
}
