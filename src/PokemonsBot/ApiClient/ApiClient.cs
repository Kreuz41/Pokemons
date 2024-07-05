using Pokemons.API.Dto.Requests;

namespace PokemonsBot.ApiClient;

public class ApiClient
{
    private const string BaseUrl = "http://api:8080/api";

    public static async Task CreateUser(long userId, CreatePlayerDto dto)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(BaseUrl);
        client.DefaultRequestHeaders.Add("userId", userId.ToString());
        await client.PostAsJsonAsync("auth/createUser", dto);
    }
}