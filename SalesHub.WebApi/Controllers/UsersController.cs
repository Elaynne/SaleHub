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

    private readonly ILogger<UsersController> _logger;
    private readonly IMediator _mediator;
    public UsersController(ILogger<UsersController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<ActionResult<IEnumerable<User>>> GetAll(Guid userId, UserRole role)
    {
        var input = new GetUsersInput()
        {
            UserId = userId,
            Role = role
        };

        var users = await _mediator.Send(input).ConfigureAwait(false);
        return Ok(users);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid userId, UserRole role)
    {
        var input = new GetUserInput()
        { 
            Id = userId,
            Role = role
        };
        var user = await _mediator.Send(input).ConfigureAwait(false);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost(Name = "CreateUser")] 
    public async Task<ActionResult<User>> CreateUser(CreateUserInput input, Guid userId, UserRole role)
    {
        if (role == UserRole.Seller && input.Role != UserRole.Client)
            return Forbid();

        var user = await _mediator.Send(input).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
    }

    [HttpPut(Name = "UpdateUser")]
    public async Task<ActionResult<User>> UpdateUser(UpdateUserInput input, Guid userId, UserRole role)
    {
        if (role == UserRole.Seller && input.User.Role != UserRole.Client)
            return Forbid();

        var user = await _mediator.Send(input).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
    }
}
