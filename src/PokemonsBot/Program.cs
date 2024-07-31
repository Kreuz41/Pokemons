using Pokemons.Core.BackgroundServices.RabbitMqListener;
using PokemonsBot.Core.Bot;
using PokemonsBot.Core.Bot.Commands.CommandHandler;
using PokemonsBot.Core.Settings;
using PokemonsBot.TransferClient.RabbitMQ;
using PokemonsDomain.MessageBroker.Models;
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

var filePath = "Resources/Preview.MP4";
using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

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
        UserId = context.ChatId
    };
    
    /*
    var photos = await context.Client.GetUserProfilePhotosAsync(context.ChatId, 
        cancellationToken: context.StoppingToken);
    if (photos.TotalCount > 0)
    {
        var currentPhoto = photos.Photos[0][0];
        var file = await context.Client.GetFileAsync(currentPhoto.FileId);
        await using var fileSavingStream = 
    }
    */

    var broker = app.Services.GetService<IBrokerSender>()!;

    await broker.Send(data);
    
    await context.Client.SendVideoAsync(context.Update.Message.Chat.Id,
        new InputFileStream(stream, filePath),
        caption: $"""
            Welcome to CIX TAP! 🎉
         
         Get ready for an exciting adventure where every click will bring you real profits. 💰 Explore the world of cryptocurrencies, catch cryptids, collect valuable assets and join clans for joint achievements. 🤝 
         
         Start your exciting journey in CIX TAP - a world where gaming and earning become one! 🌟
         
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

_ = app.Services.GetService<BotClient>();

app.Run();