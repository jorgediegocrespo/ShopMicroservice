using Shop.Messages.Bus.Commands;
using Shop.Messages.Bus.Events;

namespace Shop.Messages.Bus.Bus;

public interface IEventBus
{
	Task SendCommand<MC>(MC messageCommand) where MC : MessageCommand;
	void Publish<ME>(ME messageEvent) where ME : MessageEvent;
	void Subscribe<ME, EH>() where ME : MessageEvent where EH : IEventHandler<ME>;
}

