using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Models.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        builder.HasOne<Player>(r => r.Player)
            .WithOne(p => p.Rating)
            .HasForeignKey<Rating>(r => r.PlayerId);
    }
}