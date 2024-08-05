using System.Text.Json;
using PokemonsDomain.MessageBroker.Properties.RabbitMq;
using PokemonsDomain.MessageBroker.Sender;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PokemonsBot.TransferClient.RabbitMQ;

public class RabbitMqSender : IBrokerSender
{
    public RabbitMqSender(IConnection connection, ILogger<RabbitMqSender> logger)
    {
        _connection = connection;
        _channel = _connection.CreateChannelAsync().Result;
        _logger = logger;
    }

    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMqSender> _logger;

    public async Task Send(object obj) =>
        await Send(JsonSerializer.SerializeToUtf8Bytes(obj));

    public async Task Send(byte[] bytes)
    {
        await _channel.BasicPublishAsync("", "bot.create.player", 
            bytes, false);
    }

    public async Task<byte[]> RpsCaller(byte[]? content)
    {
        var taskSource = new TaskCompletionSource<byte[]>();
        var requestQueue = "bot.request";
        var responseQueue = "api.response";
        await _channel.QueueDeclareAsync(requestQueue, false, false, false);
        await _channel.QueueDeclareAsync(responseQueue, false, false, false);
        var correlation = Guid.NewGuid().ToString();
        var props = new BasicProperties
        {
            CorrelationId = correlation,
            ReplyTo = responseQueue
        };
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (ea.BasicProperties.CorrelationId != correlation) return;
            taskSource.SetResult(ea.Body.ToArray());
        };
        await _channel.BasicConsumeAsync(consumer, responseQueue, true);
        await _channel.BasicPublishAsync("", requestQueue, props, content);

        return await taskSource.Task;
    }
}