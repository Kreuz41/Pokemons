using Pokemons.API.Dto.Requests;
using Pokemons.DataLayer.Cache.Repository;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.PlayerRepos;

namespace Pokemons.DataLayer.MasterRepositories.PlayerRepository;

public class PlayerRepository : IPlayerRepository
{
    public PlayerRepository(ICacheRepository cacheRepository, IPlayerDatabaseRepository playerDatabaseRepository)
    {
        _cacheRepository = cacheRepository;
        _playerDatabaseRepository = playerDatabaseRepository;
    }
    
    private readonly ICacheRepository _cacheRepository;
    private readonly IPlayerDatabaseRepository _playerDatabaseRepository;

    public async Task<Player?> GetPlayerById(long id)
    {
        var player = await _cacheRepository.GetMember<Player>(id.ToString()) 
                     ?? await _playerDatabaseRepository.GetById(id);
        
        if (player is null) return null;

        await _cacheRepository.SetMember(id.ToString(), player);
        return player;
    }

    public async Task<Player> CreatePlayer(long userId, StartSessionDto dto)
    {
        var player = new Player
        {
            Name = dto.Name,
            Surname = dto.Surname,
            PhotoUrl = dto.PhotoUrl,
            Id = userId
        };

        await _playerDatabaseRepository.CreatePlayer(player);
        await _cacheRepository.SetMember(player.Id.ToString(), player);

        return player;
    }

    public async Task Save(long id)
    {
        var player = await _cacheRepository.GetMember<Player>(id.ToString());
        if (player is null) return;
        
        await _playerDatabaseRepository.UpdatePlayer(player);
        await _cacheRepository.DeleteMember<Player>(id.ToString());
    }

    public async Task Update(Player player)
    {
        await _cacheRepository.SetMember(player.Id.ToString(), player);
        await _playerDatabaseRepository.UpdatePlayer(player);
    }

    public async Task FastUpdate(Player player) =>
        await _cacheRepository.SetMember(player.Id.ToString(), player);
}