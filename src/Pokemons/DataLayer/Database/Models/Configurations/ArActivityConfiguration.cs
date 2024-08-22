using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Models.Configurations
{
    public class ArActivityConfiguration : IEntityTypeConfiguration<ArActivity>
    {
        public void Configure(EntityTypeBuilder<ArActivity> builder)
        {
            
            builder.HasKey(ps => ps.Id);

            
            builder.HasOne(ps => ps.Player)
                   .WithMany() 
                   .HasForeignKey(ps => ps.PlayerId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.Property(ps => ps.LastCoinCollectedAt)
                   .IsRequired(); 

            
            builder.Property(ps => ps.Energy)
                   .IsRequired(); 
        }
    }
}
