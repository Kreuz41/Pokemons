using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokemons.Migrations
{
    /// <inheritdoc />
    public partial class InlineToRefSystemAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Inline",
                table: "ReferralNodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inline",
                table: "ReferralNodes");
        }
    }
}
