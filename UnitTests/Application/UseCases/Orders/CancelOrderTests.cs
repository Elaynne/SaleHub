using Application.UseCases.Orders.CancelOrder;
using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Application.UseCases.Orders
{
    public class CancelOrderTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CancelOrder> _logger;
        private readonly CancelOrder _cancelOrderUseCase;

        public CancelOrderTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _logger = Substitute.For<ILogger<CancelOrder>>();
            _cancelOrderUseCase = new CancelOrder(_orderRepository, _logger);
        }

        [Fact]
        public async Task Handle_ShouldCancelOrder_WhenOrderIsPending()
        {
            var orderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                Status = OrderStatus.Pending,
                TotalPrice = 100m,
                UpdatedAt = DateTime.Now.AddHours(-1)
            };

            _orderRepository.GetOrderByIdAsync(orderId).Returns(Task.FromResult(order));
            _orderRepository.UpdateOrderAsync(Arg.Any<Order>()).Returns(Task.FromResult(order));

            var input = new CancelOrderInput { OrderId = orderId, UserId = userId };

            var result = await _cancelOrderUseCase.Handle(input, CancellationToken.None);

            result.Should().NotBeNull();
            result.OrderId.Should().Be(orderId);
            result.Status.Should().Be(OrderStatus.Cancelled.ToString());
            result.TotalAmount.Should().Be(order.TotalPrice);

            await _orderRepository.Received(1).GetOrderByIdAsync(orderId);
            await _orderRepository.Received(1).UpdateOrderAsync(Arg.Is<Order>(o => o.Status == OrderStatus.Cancelled));
        }

        [Fact]
        public async Task Handle_ShouldNotCancelOrder_WhenOrderIsNotPending()
        {
            var orderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                Status = OrderStatus.Shipped,
                OrderStatusDescription = OrderStatus.Shipped.ToString(),
                TotalPrice = 100m,
                UpdatedAt = DateTime.Now.AddHours(-1)
            };

            _orderRepository.GetOrderByIdAsync(orderId).Returns(Task.FromResult(order));

            var input = new CancelOrderInput { OrderId = orderId, UserId = userId };

            var result = await _cancelOrderUseCase.Handle(input, CancellationToken.None);

            result.Should().NotBeNull();
            result.OrderId.Should().Be(orderId);
            result.Status.Should().Be(order.Status.ToString());
            result.TotalAmount.Should().Be(order.TotalPrice);

            await _orderRepository.Received(1).GetOrderByIdAsync(orderId);
            await _orderRepository.DidNotReceive().UpdateOrderAsync(Arg.Any<Order>());
        }
    }

}
