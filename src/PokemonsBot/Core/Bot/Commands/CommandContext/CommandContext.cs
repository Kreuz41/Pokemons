using Telegram.Bot;
using Telegram.Bot.Types;

namespace PokemonsBot.Core.Bot.Commands.CommandContext;

public class CommandContext
{
    public ITelegramBotClient Client { get; set; } = null!;
    public Update Update { get; set; } = null!;
    public CancellationToken StoppingToken { get; set; }
    public long ChatId { get; set; }
}