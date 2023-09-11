using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchmakingSignalingServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitalCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameSessionName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.GameSessionId);
                });

            migrationBuilder.CreateTable(
                name: "PlayerClients",
                columns: table => new
                {
                    PlayerClientId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: false),
                    ConnectionState = table.Column<int>(type: "INTEGER", nullable: false),
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Expiration = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerClients", x => x.PlayerClientId);
                    table.ForeignKey(
                        name: "FK_PlayerClients_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "GameSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignalingSteps",
                columns: table => new
                {
                    SignalingStepId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    Target = table.Column<string>(type: "TEXT", nullable: false),
                    InformationType = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionDescription_SessionType = table.Column<string>(type: "TEXT", nullable: true),
                    SessionDescription_Sdp = table.Column<string>(type: "TEXT", nullable: true),
                    IceCandidate_Media = table.Column<string>(type: "TEXT", nullable: true),
                    IceCandidate_Index = table.Column<string>(type: "TEXT", nullable: true),
                    IceCandidate_Name = table.Column<string>(type: "TEXT", nullable: true),
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignalingSteps", x => x.SignalingStepId);
                    table.ForeignKey(
                        name: "FK_SignalingSteps_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "GameSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClients_GameSessionId",
                table: "PlayerClients",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SignalingSteps_GameSessionId",
                table: "SignalingSteps",
                column: "GameSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerClients");

            migrationBuilder.DropTable(
                name: "SignalingSteps");

            migrationBuilder.DropTable(
                name: "GameSessions");
        }
    }
}
