using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dias.tracker.api.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BotResponses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created = table.Column<DateTime>(nullable: false),
                    createdBy = table.Column<string>(nullable: true),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedBy = table.Column<string>(nullable: true),
                    isSuppressed = table.Column<bool>(nullable: false),
                    triggerText = table.Column<string>(nullable: true),
                    cooldown = table.Column<TimeSpan>(nullable: false),
                    responses = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotResponses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "HamborgText",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created = table.Column<DateTime>(nullable: false),
                    createdBy = table.Column<string>(nullable: true),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedBy = table.Column<string>(nullable: true),
                    isSuppressed = table.Column<bool>(nullable: false),
                    pool = table.Column<int>(nullable: false),
                    text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HamborgText", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created = table.Column<DateTime>(nullable: false),
                    createdBy = table.Column<string>(nullable: true),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedBy = table.Column<string>(nullable: true),
                    isSuppressed = table.Column<bool>(nullable: false),
                    discordHandle = table.Column<string>(nullable: true),
                    characterName = table.Column<string>(nullable: true),
                    xivApiId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created = table.Column<DateTime>(nullable: false),
                    createdBy = table.Column<string>(nullable: true),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedBy = table.Column<string>(nullable: true),
                    isSuppressed = table.Column<bool>(nullable: false),
                    server = table.Column<string>(nullable: true),
                    channel = table.Column<string>(nullable: true),
                    authorDiscordId = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polls", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "XivJobs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created = table.Column<DateTime>(nullable: false),
                    createdBy = table.Column<string>(nullable: true),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedBy = table.Column<string>(nullable: true),
                    isSuppressed = table.Column<bool>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XivJobs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerData",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created = table.Column<DateTime>(nullable: false),
                    createdBy = table.Column<string>(nullable: true),
                    modified = table.Column<DateTime>(nullable: false),
                    modifiedBy = table.Column<string>(nullable: true),
                    isSuppressed = table.Column<bool>(nullable: false),
                    playerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerData", x => x.id);
                    table.ForeignKey(
                        name: "FK_PlayerData_Players_playerId",
                        column: x => x.playerId,
                        principalTable: "Players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerData_playerId",
                table: "PlayerData",
                column: "playerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotResponses");

            migrationBuilder.DropTable(
                name: "HamborgText");

            migrationBuilder.DropTable(
                name: "PlayerData");

            migrationBuilder.DropTable(
                name: "Polls");

            migrationBuilder.DropTable(
                name: "XivJobs");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
