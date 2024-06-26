﻿using Microsoft.EntityFrameworkCore;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;

namespace Pokemons.DataLayer.Database.Repositories.ReferralRepos;

public class ReferralNodeDatabaseRepository : IReferralNodeDatabaseRepository
{
    public ReferralNodeDatabaseRepository(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;


    public async Task CreateNode(ReferralNode node)
    {
        await _unitOfWork.BeginTransaction();
        await _context.ReferralNodes.AddAsync(node);
        await _unitOfWork.CommitTransaction();
    }

    public async Task<IEnumerable<Player>> GetReferrals(long playerId)
    {
        var nodes = await _context.ReferralNodes
            .Where(r => r.ReferrerId == playerId)
            .Include(referralNode => referralNode.Referral)
            .ToListAsync();
        
        return nodes.Select(n => n.Referral);
    }

    public async Task<ReferralNode?> GetFirstReferralNode(long referral) =>
        await _context.ReferralNodes
            .Include(r => r.Referrer)
            .FirstOrDefaultAsync(
            r => r.ReferralId == referral && r.Inline == 1);
}