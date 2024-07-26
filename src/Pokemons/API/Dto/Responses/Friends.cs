namespace Pokemons.API.Dto.Responses;

public class Friends
{
    public List<FriendsItem> List { get; set; } = [];
    public int Total { get; set; }
    public long TotalBalance { get; set; }
}