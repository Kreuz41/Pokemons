using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Cache.Repository;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.ReferralRepos;

namespace Pokemons.DataLayer.MasterRepositories.ReferralNodeRepository;

public class ReferralNodeRepository : IReferralNodeRepository
{
    public ReferralNodeRepository(ICacheRepository cacheRepository, IReferralNodeDatabaseRepository databaseRepository)
    {
        _cacheRepository = cacheRepository;
        _databaseRepository = databaseRepository;
    }

    private readonly ICacheRepository _cacheRepository;
    private readonly IReferralNodeDatabaseRepository _databaseRepository;

    public async Task CreateNode(ReferralNode node) =>
        await _databaseRepository.CreateNode(node);

    public async Task<IEnumerable<ReferralInline>> GetReferrals(long playerId)
    {
        var referrals = await _cacheRepository.GetMember<ReferralList>(playerId.ToString());
        if (referrals is not null) return referrals.Referrals;

        referrals = new ReferralList
        {
            Referrals = await _databaseRepository.GetReferrals(playerId)
        };
        await _cacheRepository.SetMember(playerId.ToString(), referrals, 5);
        
        return referrals.Referrals;
    }

    public async Task<ReferralNode?> GetReferralNode(long playerId) => 
        await _databaseRepository.GetFirstReferralNode(playerId);

    public async Task<IEnumerable<ReferralNode>> GetParentsForPlayer(long playerId)
    {
        var parents = await _cacheRepository.GetMember<IEnumerable<ReferralNode>>(playerId.ToString());
        if (parents is not null) return parents;

        parents = await _databaseRepository.GetParentsForPlayer(playerId);
        await _cacheRepository.SetMember(playerId.ToString(), parents, 5);

        return parents;
    }

    public Task UpdateEnumerable(IEnumerable<ReferralNode> parents) =>
        _databaseRepository.UpdateRange(parents);
}