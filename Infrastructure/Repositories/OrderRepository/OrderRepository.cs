using Domain.Cache;
using Domain.Models;
using Domain.Repository.Interfaces;
using Infrastructure.Common;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories.OrderRepository
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        private readonly IMemoryCache _memoryCache;
        private const int ExpirationTimeInMinutes = 60;

        public OrderRepository(IMemoryCache memoryCache) : base(memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            order.OrderId = Guid.NewGuid();
            order.OrderDate = DateTime.UtcNow;

            _memoryCache.Set($"Order_{order.OrderId}", order, TimeSpan.FromMinutes(ExpirationTimeInMinutes));
            AppendDataOnCache(order, CacheKeys.OrdersKey, order.OrderId, ExpirationTimeInMinutes);

            return order;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var orders = GetDataSet<Order>(CacheKeys.OrdersKey);
            return (orders != null) ? orders.Values.ToList() : new List<Order>();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            if (_memoryCache.TryGetValue($"Order_{id}", out Order cachedOrder))
            {
                return cachedOrder;
            }
            throw new NotFoundException($"Order {id} not found");
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            if (_memoryCache.TryGetValue($"Order_{order.OrderId}", out Order cachedOrder))
            {
                _memoryCache.Set($"Order_{order.OrderId}", order, TimeSpan.FromMinutes(ExpirationTimeInMinutes));
                UpdateDataOnCache(order, order.OrderId, CacheKeys.OrdersKey, ExpirationTimeInMinutes);
                return order;
            }
            throw new NotFoundException($"Cannot update order data. Order {order.OrderId} not found");
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            if (_memoryCache.TryGetValue($"Order_{id}", out Order cachedOrder))
            {
                _memoryCache.Remove($"Order_{id}");
                DeleteItemOnCache<Order>(id, CacheKeys.OrdersKey, ExpirationTimeInMinutes);
                return true;
            }
            return false;
        }
    }
}
