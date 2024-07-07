using System.Text.Json;
using PokemonsDomain.MessageBroker.Sender;
using RabbitMQ.Client;

namespace PokemonsBot.TransferClient.RabbitMQ;

public class RabbitMqSender : IBrokerSender
{
    public RabbitMqSender(string rabbitPath, ILogger<RabbitMqSender> logger)
    {
        _rabbitPath = rabbitPath;
        _logger = logger;
    }

    private readonly string _rabbitPath;
    private readonly ILogger<RabbitMqSender> _logger;

    public async Task Send(object obj) =>
        await Send(JsonSerializer.SerializeToUtf8Bytes(obj));

    public async Task Send(byte[] bytes)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitPath
        };

        _logger.LogInformation($"Start to send in rabbit to {_rabbitPath}");
        
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "CreatePlayer", 
            durable: false, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null);

        await channel.BasicPublishAsync(
            exchange: "", 
            routingKey: "CreatePlayer", 
            body: bytes);
    }
}