using Contracts.IntegrationEvents.Events;
using Infrastructure.Shared.EventBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Contracts.IntegrationEvents.EventHandlers
{
    public class TravelOrderCreatedEventHandler(ILogger<TravelOrderCreatedEventHandler> logger)
        : IEventHandler<TravelOrderCreatedEvent>
    {
        public Task Handle(TravelOrderCreatedEvent @event)
        {
            logger.LogInformation($"Received {nameof(TravelOrderCreatedEvent)} " +
                $"with id {@event.Id} " +
                $"and CreatedDate {@event.CreatedDate}");
            
            return Task.CompletedTask;
        }
    }
}
