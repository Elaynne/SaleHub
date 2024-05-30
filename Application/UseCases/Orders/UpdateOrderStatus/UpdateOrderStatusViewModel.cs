using Domain.Enums;

namespace Application.UseCases.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusViewModel
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
