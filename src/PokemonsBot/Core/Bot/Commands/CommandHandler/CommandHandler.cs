namespace PokemonsBot.Core.Bot.Commands.CommandHandler;

public class CommandHandler : ICommandHandler
{
    private static IList<SlashCommand> Commands { get; } = [];
    
    public SlashCommand RegisterCommand(Handler handler)
    {
        var command = new SlashCommand().AddHandler(handler);
        Commands.Add(command);
        return command;
    }

    public async Task HandleCommand(string command, CommandContext.CommandContext context)
    {
        foreach (var slashCommand in Commands)
        {
            if(slashCommand.CanHandle(command))
                await slashCommand.Handle(context);
        }
    }
}