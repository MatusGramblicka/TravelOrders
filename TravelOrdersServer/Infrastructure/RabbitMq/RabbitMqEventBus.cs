using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text.Json;
using Infrastructure.EventBus;
using Infrastructure.EventBus.Abstractions;

namespace Infrastructure.RabbitMq;

public class RabbitMqEventBus : IEventBus
{
    private const string ExchangeName = "ecommerce-exchange";

    private readonly IRabbitMqConnection _rabbitMqConnection;
    private readonly ResiliencePipeline _pipeline;

    public RabbitMqEventBus(IRabbitMqConnection rabbitMqConnection,
        IOptions<EventBusOptions> options)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _pipeline = CreateResiliencePipeline(options.Value.RetryCount);
    }

    public Task PublishAsync(Event @event)
    {
        var routingKey = @event.GetType().Name;

        return _pipeline.Execute(() =>
        {
            using var channel = _rabbitMqConnection.Connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: "fanout",
                durable: false,
                autoDelete: false,
                null);

            var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());

            channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: null,
                body: body);

            return Task.CompletedTask;
        });
    }

    private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
    {
        var retryOptions = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>().Handle<AlreadyClosedException>(),
            BackoffType = DelayBackoffType.Exponential,
            MaxRetryAttempts = retryCount
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .Build();
    }
}