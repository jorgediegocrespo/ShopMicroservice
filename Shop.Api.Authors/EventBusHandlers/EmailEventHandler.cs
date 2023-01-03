using Shop.Messages.Bus.Bus;
using Shop.Messages.Bus.EventQueues;

namespace Shop.Api.Authors.EventBusHandlers;

public class EmailEventHandler : IEventHandler<EmailMessageEventQueue>
{
	public EmailEventHandler() { }

    public Task Handle(EmailMessageEventQueue messageEvent)
    {
        return Task.CompletedTask;
    }
}

