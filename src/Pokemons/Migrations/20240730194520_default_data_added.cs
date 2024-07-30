using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pokemons.Migrations
{
    /// <inheritdoc />
    public partial class default_data_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ActiveMissions",
                columns: new[] { "Id", "EndDate", "IsDifficult", "IsEnded", "Reward" },
                values: new object[,]
                {
                    { 1, null, false, false, 50 },
                    { 2, null, true, false, 50 },
                    { 3, null, true, false, 50 },
                    { 4, null, true, false, 50 },
                    { 5, null, true, false, 50 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ActiveMissions",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
