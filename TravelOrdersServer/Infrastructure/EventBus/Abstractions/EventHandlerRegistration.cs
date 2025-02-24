namespace Infrastructure.EventBus.Abstractions;

public class EventHandlerRegistration
{
    public Dictionary<string, Type> EventTypes { get; } = [];
}