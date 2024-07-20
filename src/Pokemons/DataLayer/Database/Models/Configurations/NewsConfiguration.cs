using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Models.Configurations;

public class NewsConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).ValueGeneratedOnAdd();

        builder.HasOne<ActiveNews>(n => n.ActiveNews)
            .WithMany(a => a.PlayerNews)
            .HasForeignKey(n => n.ActiveNewsId);

        builder.HasOne<Player>(n => n.Player)
            .WithMany(p => p.News)
            .HasForeignKey(p => p.PlayerId);
    }
}