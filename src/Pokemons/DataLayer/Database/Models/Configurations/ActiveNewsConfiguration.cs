using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Models.Configurations;

public class ActiveNewsConfiguration : IEntityTypeConfiguration<ActiveNews>
{
    public void Configure(EntityTypeBuilder<ActiveNews> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
    }
}