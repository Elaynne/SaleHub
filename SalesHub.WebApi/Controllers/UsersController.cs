using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.GetUser;
using Application.UseCases.Users.GetUsers;
using Application.UseCases.Users.UpdateUser;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SalesHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    [Authorize(Roles = "Admin, Seller")]
   // [RoleDiscoveryFilter]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var user = GetUserFromToken();
        var input = new GetUsersInput()
        {
            Role = user.Value.Role,
            SellerId = user.Value.Id
        };

        var users = await _mediator.Send(input).ConfigureAwait(false);
        return Ok(users);
    }

    private (UserRole Role, Guid? Id)? GetUserFromToken()
    {
        var bearerToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(bearerToken) as JwtSecurityToken;

        var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value;
        var userRole = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;
        var role = userRole.ToEnum<UserRole>();

        return !string.IsNullOrEmpty(userId) ? (role, Guid.Parse(userId)) : null;
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
