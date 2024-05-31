using MediatR;

namespace Application.UseCases.Orders.CancelOrder
{
    public interface ICanceloOrder : IRequestHandler<CancelOrderInput, CancelOrderOutput>
    {
    }
}
