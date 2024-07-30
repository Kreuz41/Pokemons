using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Models.Configurations;

public class ActiveMissionConfiguration : IEntityTypeConfiguration<ActiveMission>
{
    public void Configure(EntityTypeBuilder<ActiveMission> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.HasData(new List<ActiveMission>
        {
            new()
            {
                Id = 1,
                IsEnded = false,
                IsDifficult = false,
                Reward = 50
            },
            new()
            {
                Id = 2,
                IsEnded = false,
                IsDifficult = true,
                Reward = 50
            },
            new()
            {
                Id = 3,
                IsEnded = false,
                IsDifficult = true,
                Reward = 50
            },
            new()
            {
                Id = 4,
                IsEnded = false,
                IsDifficult = true,
                Reward = 50
            },
            new()
            {
                Id = 5,
                IsEnded = false,
                IsDifficult = true,
                Reward = 50
            }
        });
    }
}