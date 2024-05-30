using Application.UseCases.Books.GetBook;
using Application.UseCases.Books.GetBooks;
using Application.UseCases.Books.CreateBook;
using Application.UseCases.Books.DeleteBook;
using Application.UseCases.Books.UpdateBook;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesHub.WebApi.ActionFilterAtributes;
using Domain.Enums;

namespace SalesHub.WebApi.Controllers;

[ApiController]
[Route("api/products")]
[RoleDiscoveryFilter]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = "GetAllBooks")]
    [Authorize(Roles = "Admin, Seller")]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll()
    {
        var input = new GetBooksInput()
        {
            UserId = GetUserIdFromContext(),
            UserRole = GetUserRoleFromContext()
        };

        var books = await _mediator.Send(input).ConfigureAwait(false);
        return Ok(books);
    }


    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Seller")]
    public async Task<ActionResult<Book>> GetBook(Guid id)
    {
        var input = new GetBookInput()
        {
            UserId = GetUserIdFromContext(),
            BookId = id
        };
        var book = await _mediator.Send(input).ConfigureAwait(false);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost(Name = "CreateBook")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Book>> CreateBook(CreateBookInput input)
    {
        var book = await _mediator.Send(input).ConfigureAwait(false);

        return Ok(book);
    }

    [HttpPut(Name = "UpdateBook")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Book>> UpdateBook(UpdateBookInput input)
    {
        var book = await _mediator.Send(input).ConfigureAwait(false);

        return Ok(book);
    }
    [HttpDelete("{id}", Name = "DeleteBook")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Book>> DeleteBook(Guid id)
    {
        var input = new DeleteBookInput()
        {
            Id = id
        };
        var isDeleted = await _mediator.Send(input).ConfigureAwait(false);
        if(isDeleted)
            return Ok();
        return NotFound();
    }
    private Guid GetUserIdFromContext() => Guid.Parse(HttpContext.Items["userId"].ToString());
    private UserRole GetUserRoleFromContext() => (UserRole)HttpContext.Items["userRole"];
}
