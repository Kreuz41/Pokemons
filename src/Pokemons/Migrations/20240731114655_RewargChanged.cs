using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokemons.Migrations
{
    /// <inheritdoc />
    public partial class RewargChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Reward",
                value: 100);

            migrationBuilder.UpdateData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Reward",
                value: 100);

            migrationBuilder.UpdateData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Reward",
                value: 100);

            migrationBuilder.UpdateData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "Reward",
                value: 100);

            migrationBuilder.UpdateData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "Reward",
                value: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Reward",
                value: 50);

            migrationBuilder.UpdateData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Reward",
                value: 50);

            migrationBuilder.UpdateData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Reward",
                value: 50);

            migrationBuilder.UpdateData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "Reward",
                value: 50);

            migrationBuilder.UpdateData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "Reward",
                value: 50);
        }
    }
}
