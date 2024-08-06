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
    }

    private readonly IConnection _connection;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queue = "api.response";
        var queueRequest = "bot.request";
        using var channel = await _connection.CreateChannelAsync(stoppingToken);
        await channel.QueueDeclareAsync(queueRequest, false, false, false, cancellationToken: stoppingToken);
        await channel.QueueDeclareAsync(queue, false, false, false, cancellationToken: stoppingToken);
        var consumer = new EventingBasicConsumer(channel);
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
                    await channel.BasicPublishAsync(
                        exchange: "", 
                        routingKey: ea.BasicProperties.ReplyTo!, 
                        basicProperties: props, 
                        body: JsonSerializer.SerializeToUtf8Bytes(count),
                        cancellationToken: stoppingToken);
                    break;
            }
            
            await channel.BasicAckAsync(ea.DeliveryTag, true, stoppingToken);
        };

        await channel.BasicConsumeAsync(
            consumer: consumer,
            autoAck: false,
            queue: queueRequest);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}