using Domain.Models;

namespace Domain.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(Guid id);
    }
}
