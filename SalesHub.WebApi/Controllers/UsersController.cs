using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.GetUser;
using Application.UseCases.Users.GetUsers;
using Application.UseCases.Users.UpdateUser;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SalesHub.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
//TO-DO apply authorization
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
    //TO-DO apply User roles
    [HttpGet(Name = "Get all users")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var input = new GetUsersInput() { UserRole = Domain.Enums.UserRole.Admin };
        var users = await _mediator.Send(input).ConfigureAwait(false);
        return Ok(users);
    } 
    
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var input = new GetUserInput(){ Id = id };
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
        var user = await _mediator.Send(input).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
    }

    [HttpPut(Name = "UpdateUser")]
    public async Task<ActionResult<User>> UpdateUser(UpdateUserInput input)
    {
        var user = await _mediator.Send(input).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
    }
}
