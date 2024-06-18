using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pokemons.Migrations
{
    /// <inheritdoc />
    public partial class MarketAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentEnergy",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Energy",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "EnergyCharge",
                table: "Players",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "MarketId",
                table: "Players",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "SuperCharge",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SuperChargeActivatedTime",
                table: "Players",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "SuperChargeCooldown",
                table: "Players",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Markets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DamagePerClickCost = table.Column<int>(type: "integer", nullable: false),
                    DamagePerClickLevel = table.Column<int>(type: "integer", nullable: false),
                    EnergyCost = table.Column<int>(type: "integer", nullable: false),
                    EnergyLevel = table.Column<int>(type: "integer", nullable: false),
                    EnergyChargeCost = table.Column<int>(type: "integer", nullable: false),
                    EnergyChargeLevel = table.Column<int>(type: "integer", nullable: false),
                    SuperChargeCost = table.Column<int>(type: "integer", nullable: false),
                    SuperChargeLevel = table.Column<int>(type: "integer", nullable: false),
                    SuperChargeCooldownCost = table.Column<int>(type: "integer", nullable: false),
                    SuperChargeCooldownLevel = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Markets_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Markets_PlayerId",
                table: "Markets",
                column: "PlayerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Markets");

            migrationBuilder.DropColumn(
                name: "CurrentEnergy",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Energy",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "EnergyCharge",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "MarketId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "SuperCharge",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "SuperChargeActivatedTime",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "SuperChargeCooldown",
                table: "Players");
        }
    }
}
