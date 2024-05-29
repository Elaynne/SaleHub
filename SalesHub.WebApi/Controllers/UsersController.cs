using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.GetUser;
using Application.UseCases.Users.GetUsers;
using Application.UseCases.Users.UpdateUser;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesHub.WebApi.ActionFilterAtributes;

namespace SalesHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Seller")]
[RoleDiscoveryFilter]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var input = new GetUsersInput()
        {
            Role = GetUserRoleFromContext()
        };

        var users = await _mediator.Send(input).ConfigureAwait(false);
        return Ok(users);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var input = new GetUserInput()
        { 
            Id = id,
            Role = GetUserRoleFromContext()
        };
        var user = await _mediator.Send(input).ConfigureAwait(false);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost(Name = "CreateUser")]
    public async Task<ActionResult<User>> CreateUser(CreateUserInput input)
    {
        if (GetUserRoleFromContext() == UserRole.Seller && input.Role != UserRole.Client)
            return Forbid();

        var user = await _mediator.Send(input).ConfigureAwait(false);

        return Ok(user);
    }

    [HttpPut(Name = "UpdateUser")]
    public async Task<ActionResult<User>> UpdateUser(UpdateUserInput input)
    {
        if (GetUserRoleFromContext() == UserRole.Seller && input.User.Role != UserRole.Client)
            return Forbid();

        var user = await _mediator.Send(input).ConfigureAwait(false);

        return Ok(user);
    }
    private UserRole GetUserRoleFromContext() => (UserRole)HttpContext.Items["userRole"];
    
}
