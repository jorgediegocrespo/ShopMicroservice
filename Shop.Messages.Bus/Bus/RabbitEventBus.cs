using System.Text;
using System.Text.Json;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shop.Messages.Bus.Commands;
using Shop.Messages.Bus.Events;

namespace Shop.Messages.Bus.Bus;

public class RabbitEventBus : IEventBus
{
    private readonly IMediator _mediator;
    private readonly Dictionary<string, List<Type>> _eventHandlerList;
    private readonly List<Type> _messageEventTypeList;

    public RabbitEventBus(IMediator mediator)
    {
        _mediator = mediator;
        _eventHandlerList = new Dictionary<string, List<Type>>();
        _messageEventTypeList = new List<Type>();
    }

    public void Publish<ME>(ME messageEvent) where ME : MessageEvent
    {
        var factory = new ConnectionFactory { HostName = "shop-rabbitmq-web" };
        using (var connection = factory.CreateConnection())
        using (var model = connection.CreateModel())
        {
            var eventName = messageEvent.GetType().Name;
            model.QueueDeclare(eventName, false, false, false, null);

            var message = JsonSerializer.Serialize(messageEvent);
            var body = Encoding.UTF8.GetBytes(message);
            model.BasicPublish("", eventName, null, body);
        }
    }

    public Task SendCommand<MC>(MC messageCommand) where MC : MessageCommand
    {
        return _mediator.Send(messageCommand);
    }

    public void Subscribe<ME, EH>()
        where ME : MessageEvent
        where EH : IEventHandler<ME>
    {
        var messageEventName = typeof(ME).Name;
        var eventaHandlerType = typeof(EH);

        if (!_messageEventTypeList.Contains(typeof(ME)))
            _messageEventTypeList.Add(typeof(ME));

        if (!_eventHandlerList.ContainsKey(messageEventName))
            _eventHandlerList.Add(messageEventName, new List<Type>());

        if (_eventHandlerList[messageEventName].Any(x => x.GetType() == eventaHandlerType))
            throw new Exception($"The handler {eventaHandlerType.Name} has been previously registered by {messageEventName}");

        _eventHandlerList[messageEventName].Add(eventaHandlerType);
        var factory = new ConnectionFactory { HostName = "shop-rabbitmq-web", DispatchConsumersAsync = true };
        var connection = factory.CreateConnection();
        var model = connection.CreateModel();
        
        model.QueueDeclare(messageEventName, false, false, false, null);
        var consumer = new AsyncEventingBasicConsumer(model);
        consumer.Received += Consumer_Received;

        model.BasicConsume(messageEventName, true, consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
    {
        string? messageEventName = e.RoutingKey;
        string? serializedMessage = Encoding.UTF8.GetString(e.Body.ToArray());

        if (string.IsNullOrWhiteSpace(messageEventName))
            return;

        if (string.IsNullOrWhiteSpace(serializedMessage))
            return;

        if (!_eventHandlerList.ContainsKey(messageEventName))
            return;

        List<Type> subscriptionList = _eventHandlerList[messageEventName];
        foreach (Type subscription in subscriptionList)
        {
            object? handler = Activator.CreateInstance(subscription);
            if (handler == null)
                continue;

            Type? messageEventType = _messageEventTypeList.SingleOrDefault(x => x.Name == messageEventName);
            if (messageEventType == null)
                continue;

            var message = JsonSerializer.Deserialize(serializedMessage, messageEventType!);
            Type concretType = typeof(IEventHandler<>).MakeGenericType(messageEventType);
            await (concretType.GetMethod("Handle")!.Invoke(handler, new object[] { message! }) as Task)!;
        }
    }
}

