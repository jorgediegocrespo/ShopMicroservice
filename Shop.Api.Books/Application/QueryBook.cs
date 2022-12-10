using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Books.Models;
using Shop.Api.Books.Repository;

namespace Shop.Api.Books.Application;

public class QueryBook
{
    public class BookList : IRequest<List<BookDto>> {}
    
    public class Handler : IRequestHandler<BookList, List<BookDto>>
    {
        private readonly BookContext _BookContext;
        private readonly IMapper _mapper;

        public Handler(BookContext BookContext, IMapper mapper)
        {
            _BookContext = BookContext;
            _mapper = mapper;
        }

        public async Task<List<BookDto>> Handle(BookList request, CancellationToken cancellationToken)
        {
            var Books = await _BookContext.Books.ToListAsync(cancellationToken);
            return _mapper.Map<List<Book>, List<BookDto>>(Books);
        }
    }
}