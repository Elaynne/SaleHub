using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.GetUser;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SalesHub.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
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
    public IEnumerable<User> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new User
        {
            Id = Guid.NewGuid(),
            UserName = "",
            Email = "",
            Password = "",
            Role = Domain.Enums.Role.Admin
        })
        .ToArray();
    } 
    
    // GET: api/User/5
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

        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }
}