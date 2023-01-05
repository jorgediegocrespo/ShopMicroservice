using Shop.Messages.Bus.Bus;
using Shop.Messages.Bus.EventQueues;

namespace Shop.Api.Authors.EventBusHandlers;

public class EmailEventHandler : IEventHandler<EmailMessageEventQueue>
{
    private readonly ILogger<EmailEventHandler> _logger;

    public EmailEventHandler(ILogger<EmailEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(EmailMessageEventQueue messageEvent)
    {
        _logger.LogInformation($"To => {messageEvent.To}; Title => {messageEvent.Title}; Content => {messageEvent.Content}");
        return Task.CompletedTask;
    }
}

