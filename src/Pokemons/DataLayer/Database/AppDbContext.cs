using Microsoft.EntityFrameworkCore;
using Pokemons.DataLayer.Database.Models.Configurations;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Player> Players { get; init; } = null!;
    public DbSet<Battle> Battles { get; init; } = null!;
    public DbSet<Market> Markets { get; init; } = null!;
    public DbSet<Rating> Rating { get; init; } = null!;
    public DbSet<Mission> Missions { get; init; } = null!;
    public DbSet<ReferralNode> ReferralNodes { get; init; } = null!;
    public DbSet<ActiveMission> ActiveMissions { get; init; } = null!;
    public DbSet<Guild> Guilds { get; init; } = null!;
    public DbSet<MemberGuildStatus> MemberGuildStatus { get; init; } = null!;
    public DbSet<ActiveNews> ActiveNews { get; init; } = null!;
    public DbSet<News> News { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NewsConfiguration());
        modelBuilder.ApplyConfiguration(new GuildConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerConfiguration());
        modelBuilder.ApplyConfiguration(new BattleConfiguration());
        modelBuilder.ApplyConfiguration(new MarketConfiguration());
        modelBuilder.ApplyConfiguration(new RatingConfiguration());
        modelBuilder.ApplyConfiguration(new MissionConfiguration());
        modelBuilder.ApplyConfiguration(new ActiveNewsConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new ReferralNodeConfiguration());
        modelBuilder.ApplyConfiguration(new ActiveMissionConfiguration());
        modelBuilder.ApplyConfiguration(new MemberGuildStatusConfiguration());
    }
}