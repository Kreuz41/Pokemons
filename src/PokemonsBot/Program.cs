using System.Text.Json;
using Pokemons.Core.BackgroundServices.RabbitMqListener;
using PokemonsBot.Core.Bot;
using PokemonsBot.Core.Bot.Commands.CommandHandler;
using PokemonsBot.Core.Settings;
using PokemonsBot.TransferClient.RabbitMQ;
using PokemonsDomain.MessageBroker.Models;
using PokemonsDomain.MessageBroker.Properties.RabbitMq;
using PokemonsDomain.MessageBroker.Sender;
using RabbitMQ.Client;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

var builder = WebApplication.CreateBuilder();

if (builder.Environment.EnvironmentName == Environments.Development)
    builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddSingleton<BotClient>();

builder.Services.AddSingleton<ICommandHandler, CommandHandler>();

builder.Services.AddSingleton<IBrokerSender, RabbitMqSender>();

builder.Services.AddHostedService<RabbitMqNotificationListener>();

builder.Services.Configure<BotOption>(builder.Configuration.GetSection("BotOption"));

builder.Services.AddSingleton<IConnection>(provider =>
{
    var logger = provider.GetService<ILogger<RabbitMqListener>>()!;
    var factory = new ConnectionFactory();
    builder.Configuration.GetSection("RabbitMqConnectionFactory").Bind(factory);
    var isConnected = false;
    while (!isConnected)
    {
        try
        {
            var connection = factory.CreateConnectionAsync().Result;
            isConnected = true;
            return connection;
        }
        catch (Exception e)
        {
            logger.LogError(e?.Message);
            Task.Delay(1000).Wait();
        }
    }

    throw new ArgumentException("Message broker is invalid");
});

var app = builder.Build();

var commandHandler = app.Services.GetRequiredService<ICommandHandler>();

commandHandler.RegisterCommand(async context =>
{
    long refId = 0;
    var text = context.Update.Message!.Text ?? "";
    var query = text.Split(' ');
    if (query.Length > 1)
        if (!long.TryParse(query[1], out refId)) refId = 0;
    
    var data = new CreateUserModel
    {
        Hash = "",
        Name = context.Update.Message.Chat.FirstName,
        Surname = context.Update.Message.Chat.LastName,
        RefId = refId,
        Username = context.Update.Message.Chat.Username,
        UserId = context.ChatId,
        LangCode = context.Update.Message.From?.LanguageCode
    };

    var broker = app.Services.GetService<IBrokerSender>()!;

    await broker.Send(data);
    
    var filePath = Path.Combine(AppContext.BaseDirectory, "Resources/Preview.MP4");
    await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
    
    await context.Client.SendVideoAsync(context.Update.Message.Chat.Id,
        new InputFileStream(stream, filePath),
        caption: $"""
            Welcome to CIX TAP 🎉 

         Explore the world of cryptocurrencies, catch cryptids, collect valuable assets and join clans for joint achievements. 🤝 

         Log in to the game and start earning money right now! 🕹️
         """,
        replyMarkup: new InlineKeyboardMarkup([
            InlineKeyboardButton.WithWebApp("Play", new WebAppInfo
            {
                Url = builder.Configuration["BotOption:WebAppLink"] 
                      ?? throw new ArgumentException("Link cannot be null")
            }),
            InlineKeyboardButton.WithUrl("Community", builder.Configuration["BotOption:TgChannel"]
                                                      ?? throw new ArgumentException("Link cannot be null")), 
        ]),
        cancellationToken: context.StoppingToken);
}).AddFilter(command => command.StartsWith("start"));

commandHandler.RegisterCommand(async context =>
{
    
}).AddFilter(command => command.StartsWith("referrals"));

commandHandler.RegisterCommand(async context =>
{
    var broker = app.Services.GetService<IBrokerSender>()!;
    var response = 
        await broker.RpsCaller(JsonSerializer.SerializeToUtf8Bytes(CallRequestNames.GlobalUsers));

    var value = JsonSerializer.Deserialize<long>(response);
    
    await context.Client.SendTextMessageAsync(context.ChatId, $"Users in game now: {value}");
}).AddFilter(command => command.StartsWith("tu"));

_ = app.Services.GetService<BotClient>();

app.Run();