using FluentValidation;
using MediatR;
using Shop.Api.Cart.Models;
using Shop.Api.Cart.Repository;

namespace Shop.Api.Cart.Application;

public class NewCart
{
    public class Execute : IRequest
    {
        public DateTime CreationDate { get; set; }
        public List<string> Products { get; set; }
    }
    
    public class ExecuteValidation : AbstractValidator<Execute>
    {
        public ExecuteValidation()
        {
            RuleFor(x => x.Products).NotEmpty();
            RuleFor(x => x.CreationDate).GreaterThanOrEqualTo(new DateTime(1900,1,1));
        }
    }
    
    public class Handler : IRequestHandler<Execute>
    {
        private readonly CartContext _cartContext;

        public Handler(CartContext cartContext)
        {
            _cartContext = cartContext;
        }
        
        public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
        {
            var cartSession = new CartSession()
            {
                CreationDate = request.CreationDate
            };
            _cartContext.Session.Add(cartSession);

            var cartResult = await _cartContext.SaveChangesAsync(cancellationToken);
            if (cartResult <= 0)
                throw new Exception("The cart has not been added"); //TODO use a custom exception

            foreach (var product in request.Products)
            {
                _cartContext.SessionDetail.Add(new CartSessionDetail()
                {
                    CreationDate = DateTime.Now,
                    CartSessionId = cartSession.CartSessionId,
                    SelectedProduct = product
                });
            }

            var productsResult = await _cartContext.SaveChangesAsync(cancellationToken);
            if (productsResult > 0)
                return Unit.Value;

            throw new Exception("The products have not been added"); //TODO use a custom exception
        }
    }
}