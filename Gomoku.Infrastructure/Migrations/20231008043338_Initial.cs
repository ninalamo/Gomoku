using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gomoku.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "player",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "game",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerOneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerTwoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_game_player_PlayerOneId",
                        column: x => x.PlayerOneId,
                        principalSchema: "dbo",
                        principalTable: "player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_player_PlayerTwoId",
                        column: x => x.PlayerTwoId,
                        principalSchema: "dbo",
                        principalTable: "player",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "cell",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Column = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cell", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cell_game_GameId",
                        column: x => x.GameId,
                        principalSchema: "dbo",
                        principalTable: "game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cell_GameId",
                schema: "dbo",
                table: "cell",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_game_PlayerOneId",
                schema: "dbo",
                table: "game",
                column: "PlayerOneId");

            migrationBuilder.CreateIndex(
                name: "IX_game_PlayerTwoId",
                schema: "dbo",
                table: "game",
                column: "PlayerTwoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cell",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "game",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "player",
                schema: "dbo");
        }
    }
}
