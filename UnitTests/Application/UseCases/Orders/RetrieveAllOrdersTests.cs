using Application.UseCases.Orders.RetrieveAllOrders;
using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Application.UseCases.Orders
{
    public class RetrieveAllOrdersTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<RetrieveAllOrders> _logger;
        private readonly RetrieveAllOrders _retrieveAllOrders;

        public RetrieveAllOrdersTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _logger = Substitute.For<ILogger<RetrieveAllOrders>>();
            _retrieveAllOrders = new RetrieveAllOrders(_orderRepository, _logger);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllOrders_WhenUserRoleIsAdmin()
        {
            var input = new RetrieveAllOrdersInput { UserId = Guid.NewGuid(), UserRole = UserRole.Admin };
            var orders = GetMockOrders();
            _orderRepository.GetAllOrdersAsync().Returns(orders.ToList());

            var result = await _retrieveAllOrders.Handle(input, CancellationToken.None);

            Assert.Equal(orders.Count(), result.Count());
        }

        [Theory]
        [InlineData(UserRole.Seller)]
        [InlineData(UserRole.Client)]
        public async Task Handle_ShouldReturnSellerOrders_WhenUserRoleIsSeller(UserRole role)
        {
            var id = Guid.NewGuid();
            var input = new RetrieveAllOrdersInput { UserId = id, UserRole = role };
            var orders = GetMockOrders();
            _orderRepository.GetAllOrdersAsync().Returns(orders.ToList());

            var result = await _retrieveAllOrders.Handle(input, CancellationToken.None);
            var expectedOrders = new List<Order>();

            if (role == UserRole.Seller) 
               expectedOrders = orders.Where(x => x.SellerId == id).ToList();
            else if(role == UserRole.Client)
               expectedOrders = orders.Where(x => x.ClientId == id).ToList();

            Assert.Equal(expectedOrders.Count(), result.Count());
            Assert.All(result, order => Assert.Equal(id, order.SellerId));
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenUserRoleIsUnknown()
        {
            var input = new RetrieveAllOrdersInput { UserId = Guid.NewGuid(), UserRole = (UserRole)999 };
            var orders = GetMockOrders();
            _orderRepository.GetAllOrdersAsync().Returns(orders.ToList());

            var result = await _retrieveAllOrders.Handle(input, CancellationToken.None);

            Assert.Null(result);
        }

        private IEnumerable<Order> GetMockOrders()
        {
            return new List<Order>
        {
            new Order { OrderId = Guid.NewGuid(), SellerId = Guid.NewGuid(), ClientId = Guid.NewGuid() },
            new Order { OrderId = Guid.NewGuid(), SellerId = Guid.NewGuid(), ClientId = Guid.NewGuid() },
            new Order { OrderId = Guid.NewGuid(), SellerId = Guid.NewGuid(), ClientId = Guid.NewGuid() }
        };
        }
    }

}
