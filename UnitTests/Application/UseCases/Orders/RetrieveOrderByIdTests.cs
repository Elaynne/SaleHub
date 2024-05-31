using Application.UseCases.Orders.RetrieveOrderById;
using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Application.UseCases.Orders
{
    public class RetrieveOrderByIdTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<RetrieveOrderById> _logger;
        private readonly RetrieveOrderById _retrieveOrderByIdUseCase;

        public RetrieveOrderByIdTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _logger = Substitute.For<ILogger<RetrieveOrderById>>();
            _retrieveOrderByIdUseCase = new RetrieveOrderById(_orderRepository, _logger);
        }

        [Fact]
        public async Task Handle_ShouldReturnOrder_WhenUserRoleIsAdmin()
        {
            var input = new RetrieveOrderByIdInput { UserId = Guid.NewGuid(), OrderId = Guid.NewGuid(), UserRole = UserRole.Admin };
            var order = GetMockOrder();
            _orderRepository.GetOrderByIdAsync(input.OrderId).Returns(order);

            var result = await _retrieveOrderByIdUseCase.Handle(input, CancellationToken.None);

            result.Should().Be(order);
        }

        [Theory]
        [InlineData(UserRole.Client)]
        [InlineData(UserRole.Seller)]
        public async Task Handle_ShouldReturnOrder_WhenUserRoleIsSellerAndOrderBelongsToRole(UserRole role)
        {
            var id = Guid.NewGuid();
            var input = new RetrieveOrderByIdInput { UserId = id, OrderId = Guid.NewGuid(), UserRole = role };
            var order = role == UserRole.Seller ? GetMockOrder(sellerId: id) : GetMockOrder(clientId: id);
            _orderRepository.GetOrderByIdAsync(input.OrderId).Returns(order);

            var result = await _retrieveOrderByIdUseCase.Handle(input, CancellationToken.None);

            result.Should().Be(order);
        }

        [Theory]
        [InlineData(UserRole.Client)]
        [InlineData(UserRole.Seller)]
        public async Task Handle_ShouldReturnNull_WhenUserRoleIsSellerAndOrderDoesNotBelongToRole(UserRole role)
        {
            var id = Guid.NewGuid();
            var input = new RetrieveOrderByIdInput { UserId = id, OrderId = Guid.NewGuid(), UserRole = role };
            var order = GetMockOrder();
            _orderRepository.GetOrderByIdAsync(input.OrderId).Returns(order);

            var result = await _retrieveOrderByIdUseCase.Handle(input, CancellationToken.None);

            result.Should().BeNull();
        }

        private Order GetMockOrder(Guid? sellerId = null, Guid? clientId = null)
        {
            return new Order
            {
                OrderId = Guid.NewGuid(),
                SellerId = sellerId ?? Guid.NewGuid(),
                ClientId = clientId ?? Guid.NewGuid()
            };
        }
    }

}
