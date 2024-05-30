using Domain.Models;

namespace Application.UseCases.Orders.CreateOrder
{
    public class CreateOrderViewModel
    {
        public Guid CLientId { get; set; }
        public Guid SellerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }

        public CreateOrderViewModel()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
