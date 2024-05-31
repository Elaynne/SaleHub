using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.RetrieveUserById;
using Application.UseCases.Users.RetrieveAllUsers;
using Application.UseCases.Users.Login;
using Application.UseCases.Users.UpdateUser;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesHub.WebApi.ActionFilterAtributes;

namespace SalesHub.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginInput loginInput)
    {
        var token = await _mediator.Send(loginInput).ConfigureAwait(false);
        if (token is null)
            return Unauthorized();
        return Ok(token);
    }

    [HttpGet(Name = "GetAllUsers")]
    [RoleDiscoveryFilter]
    [Authorize(Roles = "Admin, Seller")]
    public async Task<ActionResult<IEnumerable<Domain.Models.User>>> GetAll()
    {
        var input = new RetrieveAllUsersInput()
        {
            Role = GetUserRoleFromContext()
        };

        var users = await _mediator.Send(input).ConfigureAwait(false);
        return Ok(users);
    }


    [HttpGet("{id}")]
    [RoleDiscoveryFilter]
    [Authorize(Roles = "Admin, Seller")]
    public async Task<ActionResult<Domain.Models.User>> GetUser(Guid id)
    {
        var input = new RetrieveUserByIdInput()
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
    [RoleDiscoveryFilter]
    [Authorize(Roles = "Admin, Seller")]
    public async Task<ActionResult<Domain.Models.User>> CreateUser(CreateUserInput input)
    {
        if (GetUserRoleFromContext() == UserRole.Seller && input.Role != UserRole.Client)
            return Forbid();

        var user = await _mediator.Send(input).ConfigureAwait(false);

        return Ok(user);
    }

    [HttpPut(Name = "UpdateUser")]
    [RoleDiscoveryFilter]
    [Authorize(Roles = "Admin, Seller")]
    public async Task<ActionResult<Domain.Models.User>> UpdateUser(UpdateUserInput input)
    {
        if (GetUserRoleFromContext() == UserRole.Seller && input.User.Role != UserRole.Client)
            return Forbid();

        var user = await _mediator.Send(input).ConfigureAwait(false);

        return Ok(user);
    }
    private UserRole GetUserRoleFromContext() => (UserRole)HttpContext.Items["userRole"];
    
}
