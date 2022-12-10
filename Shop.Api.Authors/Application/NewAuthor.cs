using FluentValidation;
using MediatR;
using Shop.Api.Authors.Models;
using Shop.Api.Authors.Repository;

namespace Shop.Api.Authors.Application;

public class NewAuthor
{
    public class Execute : IRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
    }

    public class ExecuteValidation : AbstractValidator<Execute>
    {
        public ExecuteValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Birthday).GreaterThanOrEqualTo(new DateTime(1900,1,1));
        }
    }

    public class Handler : IRequestHandler<Execute>
    {
        private readonly AuthorContext _authorContext;

        public Handler(AuthorContext authorContext)
        {
            _authorContext = authorContext;
        }
        
        public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
        {
            _authorContext.Authors.Add(new Author()
            {
                AuthorGuid = Convert.ToString(Guid.NewGuid()),
                Name = request.Name,
                Surname = request.Surname,
                Birthday = request.Birthday
            });

            var result = await _authorContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Unit.Value;

            throw new Exception("The author has not been added"); //TODO use a custom exception
        }
    }
}