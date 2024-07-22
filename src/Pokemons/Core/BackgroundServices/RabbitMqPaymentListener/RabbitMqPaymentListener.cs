﻿using System.Text.Json;
using Pokemons.API.Handlers;
using PokemonsDomain.MessageBroker.Models;
using PokemonsDomain.MessageBroker.Properties.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Pokemons.Core.BackgroundServices.RabbitMqPaymentListener;

public class RabbitMqPaymentListener : BackgroundService
{
    public RabbitMqPaymentListener(IConnection connection, ILogger<RabbitMqPaymentListener> logger, IServiceScopeFactory scopeFactory)
    {
        _connection = connection;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqPaymentListener> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var channel = await _connection.CreateChannelAsync(stoppingToken);

        var routing = "bot.create.player";
        
        _logger.LogInformation($"Start to listen rabbitmq by {routing} . . .");

        await channel.QueueDeclareAsync(
            queue: routing,
            durable: false,
            arguments: null,
            autoDelete: false
            );
        
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
        };

        await channel.BasicConsumeAsync(
            routing,
            true,
            consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}