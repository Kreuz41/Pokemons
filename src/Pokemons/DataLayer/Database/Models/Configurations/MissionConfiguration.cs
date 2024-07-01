using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Models.Configurations;

public class MissionConfiguration : IEntityTypeConfiguration<Mission>
{
    public void Configure(EntityTypeBuilder<Mission> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.HasOne<Player>(m => m.Player)
            .WithMany(p => p.Missions)
            .HasForeignKey(m => m.PlayerId);

        builder.HasOne<ActiveMission>(m => m.ActiveMission)
            .WithMany(a => a.Missions)
            .HasForeignKey(a => a.ActiveMissionId);
    }
}