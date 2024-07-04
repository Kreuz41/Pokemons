using Microsoft.Extensions.Options;
using PokemonsBot.Core.Bot.Commands.CommandContext;
using PokemonsBot.Core.Bot.Commands.CommandHandler;
using PokemonsBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PokemonsBot.Core.Bot;

public class BotClient
{
    public BotClient(IOptions<BotOption> options, ILogger<BotClient> logger, ICommandHandler commandHandler)
    {
        _logger = logger;
        _commandHandler = commandHandler;
        
        var client = new TelegramBotClient(options.Value.Token 
                                           ?? throw new ArgumentException("Token is null"));
        client.StartReceiving(UpdateHandler, ErrorHandler, new ReceiverOptions
        {
            AllowedUpdates = [UpdateType.Message],
            ThrowPendingUpdates = true
        });
    }

    private readonly ILogger<BotClient> _logger;
    private readonly ICommandHandler _commandHandler;
    
    private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken stoppingToken)
    {
        
        if (update.Message is null) return;
        var chatId = update.Message.Chat.Id;
        _logger.LogInformation($"Message received from {chatId}");

        var command =  "";
        if (update.Message.Text?.StartsWith('/') ?? false)
            command = update.Message.Text.TrimStart('/');
        
        var context = new CommandContext
        {
            Client = client,
            Update = update,
            StoppingToken = stoppingToken,
            ChatId = chatId
        };
        
        await _commandHandler.HandleCommand(command, context);
    }
    
    private Task ErrorHandler(ITelegramBotClient client, Exception ex, CancellationToken stoppingToken)
    {
        _logger.LogError($"{ex.Message}");

        return Task.CompletedTask;
    }
}