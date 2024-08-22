using Microsoft.EntityFrameworkCore.Migrations;

namespace Pokemons.Migrations
{
    public partial class AddUsdtBalanceAndRenameBalanceToTrxBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TRXAmount",
                table: "Wallets",
                newName: "TRXBalance");

            // Добавление нового столбца UsdtBalance
            migrationBuilder.AddColumn<decimal>(
                name: "USDTBalance",
                table: "Wallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удаление столбца UsdtBalance
            migrationBuilder.DropColumn(
                name: "USDTBalance",
                table: "Wallets");

            // Переименование столбца TRXBalance обратно в Balance
            migrationBuilder.RenameColumn(
                name: "TRXBalance",
                table: "Wallets",
                newName: "TRXAmount");
        }
    }
}
