using Shop.Messages.Bus.Events;

namespace Shop.Messages.Bus.Commands;

public abstract class MessageCommand : MessageRequest
{
    protected MessageCommand()
    {
        TimeStamp = DateTime.Now;
    }

    public DateTime TimeStamp { get; protected set; }
}

