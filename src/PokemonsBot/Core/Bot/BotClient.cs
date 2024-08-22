using Microsoft.Extensions.Options;
using PokemonsBot.Core.Bot.Commands.CommandContext;
using PokemonsBot.Core.Bot.Commands.CommandHandler;
using PokemonsBot.Core.Settings;
using PokemonsDomain.Notification;
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
        _client = client;
        client.StartReceiving(UpdateHandler, ErrorHandler, new ReceiverOptions
        {
            AllowedUpdates = [UpdateType.Message],
            ThrowPendingUpdates = true
        });
    }

    private readonly ILogger<BotClient> _logger;
    private readonly ICommandHandler _commandHandler;
    private readonly ITelegramBotClient _client;
    
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

    public async Task SendNotification(NotifyDto notify)
    {
        var message = "";

        switch (notify.NotificationType)
        {
            case NotificationType.Referral:
                message = $"""
                          + NEW REFERRAL |
                          
                          🆔{notify.ReferralName}
                          🤝For contact | [referral](tg://user?id={notify.ReferralName})
                          
                          💰Congratulations💰
                          """;
                break;
            case NotificationType.EnergyCharged:
                message = "Your energy already charged!";
                break;
            case NotificationType.SuperChargeCharged:
                message = "Your supercharge already charged!";
                break;
            case NotificationType.TopUp:
                message = $"""
                           Wallet top up successfull!
                           Amount: {notify.TopUpValue} USDT
                           """;
                break;
            case NotificationType.Withdraw:
                message = $"""
                           Withdraw successfull!
                           Amount: {notify.TopUpValue}
                           """;
                break;
            case NotificationType.Guild:
                message = $"""
                           Your guild have the new member!
                           Member Id: {notify.GuildMemberName}
                           """;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        try
        {
            await _client.SendTextMessageAsync(notify.PlayerId, message, parseMode: ParseMode.Markdown);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}