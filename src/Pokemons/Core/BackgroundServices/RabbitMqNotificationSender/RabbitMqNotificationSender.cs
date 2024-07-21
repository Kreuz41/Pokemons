using System.Collections.Concurrent;
using System.Text.Json;
using Pokemons.DataLayer.Database.Models.Entities;
using PokemonsDomain.MessageBroker.Properties.RabbitMq;
using RabbitMQ.Client;

namespace Pokemons.Core.BackgroundServices.RabbitMqNotificationSender;

public class RabbitMqNotificationSender : BackgroundService
{
    public RabbitMqNotificationSender(IConnection connection, ILogger<RabbitMqNotificationSender> logger)
    {
        _connection = connection;
        _logger = logger;
    }
    
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqNotificationSender> _logger;

    private static readonly ConcurrentQueue<Notification> Notifications = new();
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var channel = await _connection.CreateChannelAsync(stoppingToken);
        await channel.ExchangeDeclareAsync(RabbitMqExchangeNames.PlayerEventExchange, ExchangeType.Direct);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!Notifications.TryDequeue(out var notification)) continue;
            
            var bytes = JsonSerializer.SerializeToUtf8Bytes(notification);
            await channel.BasicPublishAsync(
                exchange: RabbitMqExchangeNames.PlayerEventExchange, 
                routingKey: "api.create.notify", 
                body: bytes);
        }
    }

    public static void EnqueueNotification(Notification notification) =>
        Notifications.Enqueue(notification);
}