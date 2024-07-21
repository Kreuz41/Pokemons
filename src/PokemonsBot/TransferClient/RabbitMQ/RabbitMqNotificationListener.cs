using System.Text.Json;
using Pokemons.API.Handlers;
using PokemonsBot.Core.Bot;
using PokemonsDomain.MessageBroker.Properties.RabbitMq;
using PokemonsDomain.Notification;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PokemonsBot.TransferClient.RabbitMQ;

public class RabbitMqNotificationListener : BackgroundService
{
    public RabbitMqNotificationListener(IConnection connection, ILogger<RabbitMqNotificationListener> logger, IServiceScopeFactory scopeFactory)
    {
        _connection = connection;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqNotificationListener> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var channel = await _connection.CreateChannelAsync(stoppingToken);

        await channel.ExchangeDeclareAsync(RabbitMqExchangeNames.PlayerEventExchange, ExchangeType.Direct);

        var routing = "api.create.notify";
        var queue = await channel.QueueDeclareAsync();
        
        _logger.LogInformation($"Start to listen rabbitmq by {routing} . . .");
        
        await channel.QueueBindAsync(
            queue: queue, 
            exchange: RabbitMqExchangeNames.PlayerEventExchange, 
            routingKey: routing,
            arguments: null,
            cancellationToken: stoppingToken);
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var obj = JsonSerializer.Deserialize<NotifyDto>(body);
            if (obj is null) return;

            _logger.LogInformation($"Message received. Player Id: {obj.PlayerId}");

            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetService<BotClient>()!;
            await service.SendNotification(obj);
        };

        await channel.BasicConsumeAsync(
            queue.QueueName,
            true,
            consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}