using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Books.Application;

namespace Shop.Api.Books.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Unit>> CreateBook(NewBook.Execute data)
    {
        return await _mediator.Send(data);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetBooks()
    {
        return await _mediator.Send(new QueryBook.BookList());
    }
    
    [HttpGet("{guid}")]
    public async Task<ActionResult<BookDto>> GetBook(string guid)
    {
        return await _mediator.Send(new QueryBookByGuid.BookByGuid()
        {
            BookGuid = guid
        });
    }
}