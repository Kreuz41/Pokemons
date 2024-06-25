using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokemons.Migrations
{
    /// <inheritdoc />
    public partial class RefSystemUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReferralNodes_ReferralId",
                table: "ReferralNodes");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralNodes_ReferralId",
                table: "ReferralNodes",
                column: "ReferralId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReferralNodes_ReferralId",
                table: "ReferralNodes");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralNodes_ReferralId",
                table: "ReferralNodes",
                column: "ReferralId",
                unique: true);
        }
    }
}
