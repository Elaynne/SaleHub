using MediatR;

namespace Application.UseCases.Orders.CancelOrder
{
    public class CancelOrderInput : IRequest<CancelOrderOutput>
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
    }
}
