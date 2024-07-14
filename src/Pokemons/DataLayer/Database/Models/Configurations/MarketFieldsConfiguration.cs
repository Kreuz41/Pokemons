using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Models.Configurations;

public class MarketFieldsConfiguration : IEntityTypeConfiguration<MarketField>
{
    public void Configure(EntityTypeBuilder<MarketField> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.HasData([new MarketField
        {
            Name = "DamagePerClick",
            BaseValue = 1
        }, new MarketField
        {
            // TODO: доделать
        }]);
    }
}