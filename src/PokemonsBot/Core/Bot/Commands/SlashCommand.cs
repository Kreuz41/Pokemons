namespace PokemonsBot.Core.Bot.Commands;

public delegate bool Filter(string command);
public delegate Task Handler(CommandContext.CommandContext context);

public class SlashCommand
{
    private Filter? _commandFilter;
    private Handler? _handler;

    public SlashCommand AddHandler(Handler handler)
    {
        _handler = handler;
        return this;
    }
    
    public void AddFilter(Filter filter) => _commandFilter = filter;

    public bool CanHandle(string command) => _commandFilter?.Invoke(command) ?? false;
    
    public async Task Handle(CommandContext.CommandContext context) => await _handler?.Invoke(context)!;
}