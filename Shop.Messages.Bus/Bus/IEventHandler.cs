using Shop.Messages.Bus.Events;

namespace Shop.Messages.Bus.Bus;

public interface IEventHandler
{ }

public interface IEventHandler<in T> : IEventHandler
	where T : MessageEvent
{
	Task Handle(T messageEvent);
}

