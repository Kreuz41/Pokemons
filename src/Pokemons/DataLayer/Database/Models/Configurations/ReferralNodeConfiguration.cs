using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Models.Configurations;

public class ReferralNodeConfiguration : IEntityTypeConfiguration<ReferralNode>
{
    public void Configure(EntityTypeBuilder<ReferralNode> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        builder.HasOne<Player>(r => r.Referrer)
            .WithMany(p => p.Referrals)
            .HasForeignKey(r => r.ReferrerId);

        builder.HasOne<Player>(r => r.Referral)
            .WithOne(p => p.ReferrerInfo)
            .HasForeignKey<ReferralNode>(r => r.ReferralId);
    }
}