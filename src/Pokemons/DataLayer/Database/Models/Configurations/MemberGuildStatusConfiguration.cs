using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Models.Configurations;

public class MemberGuildStatusConfiguration : IEntityTypeConfiguration<MemberGuildStatus>
{
    public void Configure(EntityTypeBuilder<MemberGuildStatus> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.HasOne<Player>(m => m.Player)
            .WithOne(p => p.GuildStatus)
            .HasForeignKey<MemberGuildStatus>(m => m.PlayerId);

        builder.HasOne<Guild>(m => m.Guild)
            .WithMany(g => g.Members)
            .HasForeignKey(m => m.GuildId);
    }
}