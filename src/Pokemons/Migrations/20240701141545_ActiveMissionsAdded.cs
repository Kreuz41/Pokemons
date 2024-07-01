using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pokemons.Migrations
{
    /// <inheritdoc />
    public partial class ActiveMissionsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDifficult",
                table: "Missions");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Missions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ActiveMissionId",
                table: "Missions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ActiveMissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDifficult = table.Column<bool>(type: "boolean", nullable: false),
                    IsEnded = table.Column<bool>(type: "boolean", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveMissions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Missions_ActiveMissionId",
                table: "Missions",
                column: "ActiveMissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_ActiveMissions_ActiveMissionId",
                table: "Missions",
                column: "ActiveMissionId",
                principalTable: "ActiveMissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missions_ActiveMissions_ActiveMissionId",
                table: "Missions");

            migrationBuilder.DropTable(
                name: "ActiveMissions");

            migrationBuilder.DropIndex(
                name: "IX_Missions_ActiveMissionId",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "ActiveMissionId",
                table: "Missions");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Missions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsDifficult",
                table: "Missions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
