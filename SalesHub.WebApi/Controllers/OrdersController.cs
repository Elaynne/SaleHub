using Application.UseCases.Orders.CreateOrder;
using Application.UseCases.Orders.GetOrder;
using Application.UseCases.Orders.GetOrders;
using Application.UseCases.Orders.CancelOrder;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesHub.WebApi.ActionFilterAtributes;

namespace SalesHub.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RoleDiscoveryFilter]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrdersController(IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetAllOrders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var input = new GetOrdersInput()
            {
                UserId = GetUserIdFromContext(),
                UserRole = GetUserRoleFromContext(),
            };
            var orders = await _mediator.Send(input).ConfigureAwait(false);
            return Ok(orders);
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await _mediator.Send(new GetOrderInput { 
                OrderId = id,
                UserId = GetUserIdFromContext(),
                UserRole = GetUserRoleFromContext()
            }).ConfigureAwait(false);

            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost(Name = "CreateOrder")]
        [Authorize(Roles = "Admin, Seller")]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderViewModel viewModel)
        {
            var input = _mapper.Map<CreateOrderInput>(viewModel);
            input.UserId = GetUserIdFromContext();

            var order = await _mediator.Send(input).ConfigureAwait(false);

            if (order is null)
                return BadRequest("Some of the items on your order is out of stock");

            return Ok(order);
        }

        [HttpDelete("{id}", Name = "CancelOrder")]
        [Authorize(Roles = "Admin, Seller")]
        public async Task<ActionResult<Order>> CanceloOrder(Guid id)
        {
            var input = new CancelOrderInput()
            {
                OrderId = id,
                UserId = GetUserIdFromContext()
            };

            var order = await _mediator.Send(input).ConfigureAwait(false);
            return Ok(order);
        }

        private UserRole GetUserRoleFromContext() => (UserRole)HttpContext.Items["userRole"];
        private Guid GetUserIdFromContext() => Guid.Parse(HttpContext.Items["userId"].ToString());
    }
}
