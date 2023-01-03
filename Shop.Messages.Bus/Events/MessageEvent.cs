namespace Shop.Messages.Bus.Events;

public abstract class MessageEvent
{
    public MessageEvent()
    {
	    TimeStamp = DateTime.Now;
    }

    public DateTime TimeStamp { get; protected set; }
}

