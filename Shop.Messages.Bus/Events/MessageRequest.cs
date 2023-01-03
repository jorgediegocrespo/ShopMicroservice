using MediatR;

namespace Shop.Messages.Bus.Events;

public abstract class MessageRequest : IRequest<bool>
{
	public MessageRequest()
	{
		MessageType = GetType().Name;
	}

	public string MessageType { get; protected set; }
}

