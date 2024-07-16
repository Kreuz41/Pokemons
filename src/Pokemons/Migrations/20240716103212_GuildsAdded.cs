using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pokemons.Migrations
{
    /// <inheritdoc />
    public partial class GuildsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayersCount = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<int>(type: "integer", nullable: false),
                    TotalBalance = table.Column<int>(type: "integer", nullable: false),
                    GuildMasterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberGuildStatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MemberStatus = table.Column<int>(type: "integer", nullable: false),
                    GuildId = table.Column<long>(type: "bigint", nullable: true),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberGuildStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberGuildStatus_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MemberGuildStatus_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberGuildStatus_GuildId",
                table: "MemberGuildStatus",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberGuildStatus_PlayerId",
                table: "MemberGuildStatus",
                column: "PlayerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberGuildStatus");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
