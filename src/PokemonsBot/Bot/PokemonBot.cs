using Pokemons.API.Dto.Requests;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PokemonsBot.Bot;

public class PokemonBot
{
    public PokemonBot(string token)
    {
        _client = new TelegramBotClient(token);
        _client.StartReceiving(UpdateHandler, ErrorHandler, new ReceiverOptions
        {
            AllowedUpdates = [UpdateType.Message],
            ThrowPendingUpdates = true
        });
    }
    
    private readonly ITelegramBotClient _client;

    private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken stoppingToken)
    {
        if (update.Message?.Text is null) return;
        
        if (update.Message.Text.StartsWith("/start"))
        {
            long refId = 0;
            var text = update.Message.Text!;
            var query = text.Split('?');
            if (query.Length > 1) query = query[1].Split('=');
            if (query.Length > 1)
                if (!long.TryParse(query[1], out refId)) refId = 0;
            
            await ApiClient.ApiClient.CreateUser(update.Message.Chat.Id, new CreatePlayerDto
            {
                Hash = "",
                Name = update.Message.Chat.FirstName,
                Surname = update.Message.Chat.LastName,
                PhotoUrl = update.Message.Chat.Photo?.BigFileId,
                RefId = refId
            });
            
            await _client.SendTextMessageAsync(update.Message.Chat.Id,
                $"Welcome to pokemons",
                replyMarkup: new InlineKeyboardMarkup([
                    InlineKeyboardButton.WithWebApp("open", new WebAppInfo
                    {
                        Url = "https://yandex.ru/search/?text=Как+запустить+c%23+консольно+приложение+с+параметрами+через+Docker&lr=43&clid=2437996"
                    }), 
                ]),
                cancellationToken: stoppingToken);
        }
    }
    
    private async Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        Console.WriteLine(arg2);
    }
}