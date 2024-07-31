using System.Text.Json;
using PokemonsDomain.MessageBroker.Properties.RabbitMq;
using PokemonsDomain.MessageBroker.Sender;
using RabbitMQ.Client;

namespace PokemonsBot.TransferClient.RabbitMQ;

public class RabbitMqSender : IBrokerSender
{
    public RabbitMqSender(IConnection connection, ILogger<RabbitMqSender> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqSender> _logger;

    public async Task Send(object obj) =>
        await Send(JsonSerializer.SerializeToUtf8Bytes(obj));

    public async Task Send(byte[] bytes)
    {
        using var channel = await _connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(RabbitMqExchangeNames.PlayerEventExchange, ExchangeType.Direct);
        await channel.BasicPublishAsync(RabbitMqExchangeNames.PlayerEventExchange, "bot.create.player", bytes, false);
    }
}