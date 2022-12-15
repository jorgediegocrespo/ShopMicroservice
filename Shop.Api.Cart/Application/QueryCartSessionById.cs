using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Cart.RemoteServices;
using Shop.Api.Cart.Repository;

namespace Shop.Api.Cart.Application;

public class QueryCartSessionById
{
    public class CartSessionById : IRequest<CartSessionDto>
    {
        public int CartSessionId { get; set; }
    }
    
    public class Handler : IRequestHandler<CartSessionById, CartSessionDto>
    {
        private readonly CartContext _cartContext;
        private readonly IBookService _bookService;

        public Handler(CartContext cartContext, IBookService bookService)
        {
            _cartContext = cartContext;
            _bookService = bookService;
        }

        public async Task<CartSessionDto> Handle(CartSessionById request, CancellationToken cancellationToken)
        {
            var cartSession = await _cartContext.Session.FirstOrDefaultAsync(x => x.CartSessionId == request.CartSessionId, cancellationToken);
            var cartSessionDetail = await _cartContext.SessionDetail.Where(x => x.CartSessionId == request.CartSessionId).ToListAsync(cancellationToken);

            List<CartSessionDetailDto> cartSessionDetailDtos = new List<CartSessionDetailDto>();

            foreach (var detail in cartSessionDetail)
            {
                var bookResponse = await _bookService.GetBook(new Guid(detail.SelectedProduct));
                if (!bookResponse.result)
                    continue;
                
                cartSessionDetailDtos.Add(new CartSessionDetailDto()
                {
                    BookId = bookResponse.book.BookGuid,
                    BookTitle = bookResponse.book.Title,
                    PublishDate = bookResponse.book.PublishDate
                });
            }

            return new CartSessionDto()
            {
                CartSessionId = cartSession.CartSessionId,
                SessionCreationDate = cartSession.CreationDate,
                Details = cartSessionDetailDtos
            };
        }
    }
}