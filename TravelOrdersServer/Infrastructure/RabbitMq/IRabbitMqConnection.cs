using RabbitMQ.Client;

namespace Infrastructure.RabbitMq;

public interface IRabbitMqConnection
{
     IConnection Connection { get; }
}