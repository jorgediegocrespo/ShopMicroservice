using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Authors.Models;
using Shop.Api.Authors.Repository;

namespace Shop.Api.Authors.Application;

public class QueryAuthorByGuid
{
    public class AuthorByGuid : IRequest<AuthorDto>
    {
        public string AuthorGuid { get; set; }
    }
    
    public class Handler : IRequestHandler<AuthorByGuid, AuthorDto>
    {
        private readonly AuthorContext _authorContext;
        private readonly IMapper _mapper;

        public Handler(AuthorContext authorContext, IMapper mapper)
        {
            _authorContext = authorContext;
            _mapper = mapper;
        }
        
        public async Task<AuthorDto> Handle(AuthorByGuid request, CancellationToken cancellationToken)
        {
            var author = await _authorContext.Authors.Where(x => x.AuthorGuid == request.AuthorGuid).FirstOrDefaultAsync(cancellationToken);
            if (author == null)
                throw new Exception("Author not found"); //TODO use a custom exception 

            return _mapper.Map<Author, AuthorDto>(author);
        }
    }
}