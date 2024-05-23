using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.GetUser;
using Application.UseCases.Users.GetUsers;
using Application.UseCases.Users.UpdateUser;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesHub.WebApi.ActionFilterAtributes;
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
    ////TO-DO apply User roles
    //[HttpGet(Name = "GetAllUsers")]
    //[Authorize(Roles = "Admin, Seller")]
    //[RoleDiscoveryFilter]
    //public async Task<ActionResult<IEnumerable<User>>> GetAll(string userRole, Guid? sellerId = null)
    //{
    //    var input = new GetUsersInput() { 
    //        UserRole = (Domain.Enums.UserRole)Enum.Parse(typeof(Domain.Enums.UserRole), userRole),
    //        SellerId = sellerId
    //    };

    //    var users = await _mediator.Send(input).ConfigureAwait(false);
    //    return Ok(users);
    //}

    [HttpGet(Name = "GetAllUsers")]
    [Authorize(Roles = "Admin, Seller")]
   // [RoleDiscoveryFilter]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var userId = GetUserIdFromToken();
        var input = new GetUsersInput()
        {
            SellerId = userId
        };

        var users = await _mediator.Send(input).ConfigureAwait(false);
        return Ok(users);
    }

    private Guid GetUserIdFromToken()
    {
        var bearerToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(bearerToken) as JwtSecurityToken;

        var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
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
