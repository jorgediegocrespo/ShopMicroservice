using FluentValidation;
using MediatR;
using Shop.Api.Books.Models;
using Shop.Api.Books.Repository;
using Shop.Messages.Bus.Bus;
using Shop.Messages.Bus.EventQueues;

namespace Shop.Api.Books.Application;

public class NewBook
{
    public class Execute : IRequest
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public Guid AuthorGuid { get; set; }
    }

    public class ExecuteValidation : AbstractValidator<Execute>
    {
        public ExecuteValidation()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.AuthorGuid).NotEmpty();
            RuleFor(x => x.PublishDate).GreaterThanOrEqualTo(new DateTime(1900,1,1));
        }
    }

    public class Handler : IRequestHandler<Execute>
    {
        private readonly BookContext _bookContext;
        private readonly IEventBus _eventBus;

        public Handler(BookContext bookContext, IEventBus eventBus)
        {
            _bookContext = bookContext;
            _eventBus = eventBus;
        }

        public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
        {
            _bookContext.Books.Add(new Book()
            {
                BookGuid = Guid.NewGuid(),
                Title = request.Title,
                AuthorGuid = request.AuthorGuid,
                PublishDate = request.PublishDate
            });

            var result = await _bookContext.SaveChangesAsync(cancellationToken);
            if (result <= 0)
                throw new Exception("The book has not been added"); //TODO use a custom exception

            _eventBus.Publish(new EmailMessageEventQueue("jorgediegocrespo@gmail.com", request.Title, "This is a test"));
            return Unit.Value;            
        }
    }
}