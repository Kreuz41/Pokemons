using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pokemons.DataLayer.Database;

namespace Pokemons.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240821123456_AddUsdtBalanceAndRenameBalanceToTrxBalance")] // Убедитесь, что ID миграции правильный
    public partial class AddUsdtBalanceAndRenameBalanceToTrxBalance : Migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            // Другие сущности модели

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Wallet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("TRXBalance") // Переименованное поле
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("USDTBalance") // Новое поле для USDT
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Wallets");
                });

            // Остальные части модели
        }
    }
}
