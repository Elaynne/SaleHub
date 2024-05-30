using Domain.Models;

namespace Application.UseCases.Orders.CreateOrder
{
    public class CreateOrderViewModel
    {
        public Guid CLientId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public CreateOrderViewModel()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
