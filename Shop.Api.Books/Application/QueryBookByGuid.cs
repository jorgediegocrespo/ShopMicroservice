using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Books.Models;
using Shop.Api.Books.Repository;

namespace Shop.Api.Books.Application;

public class QueryBookByGuid
{
    public class BookByGuid : IRequest<BookDto>
    {
        public string BookGuid { get; set; }
    }
    
    public class Handler : IRequestHandler<BookByGuid, BookDto>
    {
        private readonly BookContext _BookContext;
        private readonly IMapper _mapper;

        public Handler(BookContext BookContext, IMapper mapper)
        {
            _BookContext = BookContext;
            _mapper = mapper;
        }
        
        public async Task<BookDto> Handle(BookByGuid request, CancellationToken cancellationToken)
        {
            var guid = Guid.Parse(request.BookGuid);
            var Book = await _BookContext.Books.Where(x => x.BookGuid == guid).FirstOrDefaultAsync(cancellationToken);
            if (Book == null)
                throw new Exception("Book not found"); //TODO use a custom exception 

            return _mapper.Map<Book, BookDto>(Book);
        }
    }
}