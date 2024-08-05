using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Pokemons.DataLayer.Database;
using PokemonsDomain.MessageBroker.Properties.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Pokemons.Core.BackgroundServices.BotRequestsListener;

public class BotRequestsListener : BackgroundService
{
    public BotRequestsListener(IConnection connection, IDbContextFactory<AppDbContext> contextFactory)
    {
        _connection = connection;
        _contextFactory = contextFactory;
        _channel = connection.CreateChannelAsync().Result;
    }

    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queue = "api.response";
        var queueRequest = "bot.request";
        await _channel.QueueDeclareAsync(queueRequest, false, false, false, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(queue, false, false, false, cancellationToken: stoppingToken);
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (message, ea) =>
        {
            await using var context = await _contextFactory.CreateDbContextAsync(stoppingToken);
            var request = Encoding.UTF8.GetString(ea.Body.ToArray()).Trim('"');
            var props = new BasicProperties
            {
                CorrelationId = ea.BasicProperties.CorrelationId
            };
            switch (request)
            {
                case $"{CallRequestNames.GlobalUsers}":
                    var count = context.Players.Count();
                    await _channel.BasicPublishAsync(
                        exchange: "", 
                        routingKey: queue, 
                        basicProperties: props, 
                        body: JsonSerializer.SerializeToUtf8Bytes(count),
                        cancellationToken: stoppingToken);
                    break;
            }
        };

        await _channel.BasicConsumeAsync(
            consumer: consumer,
            autoAck: true,
            queue: queueRequest);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}