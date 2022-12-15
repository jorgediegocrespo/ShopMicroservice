using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Cart.Application;

namespace Shop.Api.Cart.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Unit>> CreateCart(NewCart.Execute data)
    {
        return await _mediator.Send(data);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CartSessionDto>> GetCartSession(int id)
    {
        return await _mediator.Send(new QueryCartSessionById.CartSessionById()
        {
            CartSessionId = id
        });
    }
}