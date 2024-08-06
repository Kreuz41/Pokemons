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
        _logger = logger;
    }

    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqSender> _logger;

    public async Task Send(object obj) =>
        await Send(JsonSerializer.SerializeToUtf8Bytes(obj));

    public async Task Send(byte[] bytes)
    {
        var channel = await _connection.CreateChannelAsync();
        await channel.BasicPublishAsync("", "bot.create.player", 
            bytes, false);
    }

    public async Task<byte[]> RpsCaller(byte[]? content)
    {
        var taskSource = new TaskCompletionSource<byte[]>();
        var requestQueue = "bot.request";
        var responseQueue = "api.response";
        using var channel = await _connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(requestQueue, false, false, false);
        await channel.QueueDeclareAsync(responseQueue, false, false, false);
        var correlation = Guid.NewGuid().ToString();
        var props = new BasicProperties
        {
            CorrelationId = correlation,
            ReplyTo = responseQueue
        };
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            if (ea.BasicProperties.CorrelationId != correlation) return;
            taskSource.SetResult(ea.Body.ToArray());
        };
        
        await channel.BasicConsumeAsync(consumer, responseQueue, true);
        await channel.BasicPublishAsync("", requestQueue, props, content);
        
        var consumerTag = await channel.BasicConsumeAsync(responseQueue, true, consumer);
        var response = await taskSource.Task;
        await channel.BasicCancelAsync(consumerTag);

        return response;
    }
}