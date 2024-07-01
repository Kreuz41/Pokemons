using Microsoft.EntityFrameworkCore;
using Pokemons.DataLayer.Database.Models.Configurations;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Battle> Battles { get; set; } = null!;
    public DbSet<Market> Markets { get; set; } = null!;
    public DbSet<Rating> Rating { get; set; } = null!;
    public DbSet<Mission> Missions { get; set; } = null!;
    public DbSet<ReferralNode> ReferralNodes { get; set; } = null!;
    public DbSet<ActiveMission> ActiveMissions { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PlayerConfiguration());
        modelBuilder.ApplyConfiguration(new BattleConfiguration());
        modelBuilder.ApplyConfiguration(new MarketConfiguration());
        modelBuilder.ApplyConfiguration(new RatingConfiguration());
        modelBuilder.ApplyConfiguration(new MissionConfiguration());
        modelBuilder.ApplyConfiguration(new ReferralNodeConfiguration());
        modelBuilder.ApplyConfiguration(new ActiveMissionConfiguration());
    }
}