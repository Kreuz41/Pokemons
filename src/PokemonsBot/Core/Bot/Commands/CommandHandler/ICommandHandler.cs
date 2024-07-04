namespace PokemonsBot.Core.Bot.Commands.CommandHandler;

public interface ICommandHandler
{
    SlashCommand RegisterCommand(Handler handler);
    Task HandleCommand(string command, CommandContext.CommandContext context);
}