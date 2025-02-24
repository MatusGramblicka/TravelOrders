using Infrastructure.Shared.EventBus;

namespace Contracts.IntegrationEvents.Events;

public record TravelOrderCreatedEvent(int TravelOrderId) : Event;
