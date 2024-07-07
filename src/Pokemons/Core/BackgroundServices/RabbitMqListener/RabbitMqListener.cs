using System.Text.Json;
using Pokemons.API.Handlers;
using PokemonsDomain.MessageBroker.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Pokemons.Core.BackgroundServices.RabbitMqListener;

public class RabbitMqListener : BackgroundService
{
    private readonly ILogger<RabbitMqListener> _logger;

    private readonly string _rabbitPath;
    private readonly IServiceScopeFactory _scopeFactory;

    public RabbitMqListener(string rabbitPath, IServiceScopeFactory scopeFactory, ILogger<RabbitMqListener> logger)
    {
        _rabbitPath = rabbitPath;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start to listen rabbitmq from {_rabbitPath}...");

        var factory = new ConnectionFactory
        {
            HostName = _rabbitPath
        };

        var isConnected = false;
        while (!stoppingToken.IsCancellationRequested && !isConnected)
        {
            try
            {
                using var connection = await factory.CreateConnectionAsync(stoppingToken);
                using var channel = await connection.CreateChannelAsync(stoppingToken);

                await channel.QueueDeclareAsync(
                    "CreatePlayer",
                    false,
                    false,
                    false,
                    null,
                    cancellationToken: stoppingToken);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (sender, args) =>
                {
                    var body = args.Body.ToArray();
                    var obj = JsonSerializer.Deserialize<CreateUserModel>(body);
                    if (obj is null) return;

                    _logger.LogInformation($"Message received: {obj.UserId}");

                    using var scope = _scopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetService<IAuthHandler>()!;
                    await service.CreateUser(obj, obj.UserId);
                };

                await channel.BasicConsumeAsync(
                    "CreatePlayer",
                    true,
                    consumer);

                isConnected = true;
                
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}