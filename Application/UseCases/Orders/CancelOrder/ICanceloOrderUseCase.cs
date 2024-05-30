
using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.CancelOrder
{
    public interface ICanceloOrderUseCase : IRequestHandler<CancelOrderInput, Order>
    {
    }
}
