using System.Text.Json;
using Pokemons.API.Handlers;
using PokemonsDomain.MessageBroker.Models;
using PokemonsDomain.MessageBroker.Properties.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Pokemons.Core.BackgroundServices.RabbitMqListener;

public class RabbitMqListener : BackgroundService
{
    private readonly IConnection _connection;

    private readonly ILogger<RabbitMqListener> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    public RabbitMqListener(IServiceScopeFactory scopeFactory, ILogger<RabbitMqListener> logger,
        IConnection connection)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _connection = connection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var channel = await _connection.CreateChannelAsync(stoppingToken);

        await channel.ExchangeDeclareAsync(RabbitMqExchangeNames.PlayerEventExchange, ExchangeType.Direct);

        var routing = "bot.create.player";
        var queue = await channel.QueueDeclareAsync(routing,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);
        
        _logger.LogInformation($"Start to listen rabbitmq by {routing} . . .");
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var obj = JsonSerializer.Deserialize<CreateUserModel>(body);
            if (obj is null) return;

            _logger.LogInformation($"Message received. Object Id: {obj.UserId}");

            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetService<IAuthHandler>()!;
            await service.CreateUser(obj, obj.UserId);
            
            await channel!.BasicAckAsync(deliveryTag: args.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
        };

        await channel.BasicConsumeAsync(
            queue.QueueName,
            false,
            consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}