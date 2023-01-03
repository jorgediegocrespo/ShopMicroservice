using System;
using Shop.Messages.Bus.Events;

namespace Shop.Messages.Bus.EventQueues;

public class EmailMessageEventQueue : MessageEvent
{
    public EmailMessageEventQueue(string to, string title, string content)
    {
        To = to;
        Title = title;
        Content = content;
    }

    public string To { get; set; }
	public string Title { get; set; }
	public string Content { get; set; }
}

