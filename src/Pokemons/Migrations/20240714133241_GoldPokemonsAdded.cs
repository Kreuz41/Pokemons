using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokemons.Migrations
{
    /// <inheritdoc />
    public partial class GoldPokemonsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntityType",
                table: "Battles",
                newName: "EntityTypeId");

            migrationBuilder.AddColumn<long>(
                name: "GoldBalance",
                table: "Players",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsGold",
                table: "Battles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoldBalance",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "IsGold",
                table: "Battles");

            migrationBuilder.RenameColumn(
                name: "EntityTypeId",
                table: "Battles",
                newName: "EntityType");
        }
    }
}
