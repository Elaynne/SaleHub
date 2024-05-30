
using Domain.Models;
using MediatR;

namespace Application.UseCases.Orders.UpdateOrderStatus
{
    public interface IUpdateOrderStatusUseCase : IRequestHandler<UpdateOrderStatusInput, Order>
    {
    }
}
