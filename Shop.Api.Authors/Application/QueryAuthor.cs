using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Authors.Models;
using Shop.Api.Authors.Repository;

namespace Shop.Api.Authors.Application;

public class QueryAuthor
{
    public class AuthorList : IRequest<List<AuthorDto>> {}
    
    public class Handler : IRequestHandler<AuthorList, List<AuthorDto>>
    {
        private readonly AuthorContext _authorContext;
        private readonly IMapper _mapper;

        public Handler(AuthorContext authorContext, IMapper mapper)
        {
            _authorContext = authorContext;
            _mapper = mapper;
        }

        public async Task<List<AuthorDto>> Handle(AuthorList request, CancellationToken cancellationToken)
        {
            var authors = await _authorContext.Authors.ToListAsync(cancellationToken);
            return _mapper.Map<List<Author>, List<AuthorDto>>(authors);
        }
    }
}