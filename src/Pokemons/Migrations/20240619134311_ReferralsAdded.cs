using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pokemons.Migrations
{
    /// <inheritdoc />
    public partial class ReferralsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketId",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "SuperChargeActivatedTime",
                table: "Players",
                newName: "LastSuperChargeActivatedTime");

            migrationBuilder.RenameColumn(
                name: "CurrentEnergy",
                table: "Players",
                newName: "TotalDamage");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCommitDamageTime",
                table: "Players",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ReferralNodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReferrerId = table.Column<long>(type: "bigint", nullable: false),
                    ReferralId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferralNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferralNodes_Players_ReferralId",
                        column: x => x.ReferralId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferralNodes_Players_ReferrerId",
                        column: x => x.ReferrerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferralNodes_ReferralId",
                table: "ReferralNodes",
                column: "ReferralId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReferralNodes_ReferrerId",
                table: "ReferralNodes",
                column: "ReferrerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferralNodes");

            migrationBuilder.DropColumn(
                name: "LastCommitDamageTime",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "TotalDamage",
                table: "Players",
                newName: "CurrentEnergy");

            migrationBuilder.RenameColumn(
                name: "LastSuperChargeActivatedTime",
                table: "Players",
                newName: "SuperChargeActivatedTime");

            migrationBuilder.AddColumn<long>(
                name: "MarketId",
                table: "Players",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
