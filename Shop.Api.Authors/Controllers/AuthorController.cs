using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Authors.Application;
using Shop.Api.Authors.Models;

namespace Shop.Api.Authors.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Unit>> CreateAuthor(NewAuthor.Execute data)
    {
        return await _mediator.Send(data);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<AuthorDto>>> GetAuthors()
    {
        return await _mediator.Send(new QueryAuthor.AuthorList());
    }
    
    [HttpGet("{guid}")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(string guid)
    {
        return await _mediator.Send(new QueryAuthorByGuid.AuthorByGuid()
        {
            AuthorGuid = guid
        });
    }
}