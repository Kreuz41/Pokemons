using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokemons.Migrations
{
    /// <inheritdoc />
    public partial class CompleteTimeIsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CompleteTime",
                table: "Missions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "DamagePerClickNextValue",
                table: "Markets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "EnergyChargeNextValue",
                table: "Markets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "EnergyNextValue",
                table: "Markets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SuperChargeCooldownNextValue",
                table: "Markets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SuperChargeNextValue",
                table: "Markets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DamagePerClickNextValue",
                table: "Markets");

            migrationBuilder.DropColumn(
                name: "EnergyChargeNextValue",
                table: "Markets");

            migrationBuilder.DropColumn(
                name: "EnergyNextValue",
                table: "Markets");

            migrationBuilder.DropColumn(
                name: "SuperChargeCooldownNextValue",
                table: "Markets");

            migrationBuilder.DropColumn(
                name: "SuperChargeNextValue",
                table: "Markets");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompleteTime",
                table: "Missions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
