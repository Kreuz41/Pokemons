using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokemons.Migrations
{
    /// <inheritdoc />
    public partial class RefsCountAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RefsCount",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefsCount",
                table: "Players");
        }
    }
}
