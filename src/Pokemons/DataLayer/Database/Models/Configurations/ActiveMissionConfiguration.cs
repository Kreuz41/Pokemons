using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Models.Configurations;

public class ActiveMissionConfiguration : IEntityTypeConfiguration<ActiveMission>
{
    public void Configure(EntityTypeBuilder<ActiveMission> builder)
    {
        builder.HasKey(a => a.Id);
    }
}