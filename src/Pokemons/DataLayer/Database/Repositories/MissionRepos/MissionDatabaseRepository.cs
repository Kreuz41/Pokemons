﻿using Microsoft.EntityFrameworkCore;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;

namespace Pokemons.DataLayer.Database.Repositories.MissionRepos;

public class MissionDatabaseRepository : IMissionDatabaseRepository
{
    public MissionDatabaseRepository(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<IEnumerable<Mission>> GetAllMissions(long playerId) =>
        await _context.Missions
            .Include(m => m.ActiveMission)
            .Where(m => m.PlayerId == playerId)
            .ToListAsync();

    public async Task<Mission?> GetMission(int missionId) =>
        await _context.Missions.FirstOrDefaultAsync(m => m.Id == missionId);

    public async Task UpdateMission(Mission mission)
    {
        await _unitOfWork.BeginTransaction();
        _context.Missions.Update(mission);
        await _unitOfWork.CommitTransaction();
    }

    public async Task<IEnumerable<ActiveMission>> GetActiveMissions() =>
        await _context.ActiveMissions.Where(a => !a.IsEnded).ToListAsync();

    public async Task SaveMissions(IEnumerable<Mission> missions)
    {
        await _unitOfWork.BeginTransaction();
        await _context.Missions.AddRangeAsync(missions);
        await _unitOfWork.CommitTransaction();
    }
}