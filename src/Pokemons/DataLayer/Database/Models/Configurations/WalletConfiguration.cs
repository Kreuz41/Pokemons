using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Models.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasKey(w => w.Id);
            
            builder.Property(w => w.TRXBalance)
                   .HasColumnType("decimal(18,2)")  // Определяем точность для хранения суммы TRX
                   .IsRequired();
                   
             builder.Property(w => w.USDTBalance)
                   .HasColumnType("decimal(18,2)")  // Определяем точность для хранения суммы TRX
                   .IsRequired();

            builder.HasOne(w => w.Player)
                   .WithMany() // Если у игрока может быть несколько кошельков, укажите WithMany(w => w.Wallets)
                   .HasForeignKey(w => w.PlayerId)
                   .OnDelete(DeleteBehavior.Cascade); // При удалении игрока удаляем его кошелек
        }
    }
}
