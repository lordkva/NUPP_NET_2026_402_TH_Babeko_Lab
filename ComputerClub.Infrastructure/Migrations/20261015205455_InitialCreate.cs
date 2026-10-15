using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerClub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Computers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Processor = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RamGb = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    DomainId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Computers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    DomainId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GamingComputers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VideoCard = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RefreshRate = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamingComputers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamingComputers_Computers_Id",
                        column: x => x.Id,
                        principalTable: "Computers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfficeComputers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OfficeSoftware = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeComputers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeComputers_Computers_Id",
                        column: x => x.Id,
                        principalTable: "Computers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamingSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ComputerId = table.Column<int>(type: "INTEGER", nullable: false),
                    DomainId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamingSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamingSessions_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GamingSessions_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamingComputerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GamingComputerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Keyboard = table.Column<string>(type: "TEXT", nullable: false),
                    Mouse = table.Column<string>(type: "TEXT", nullable: false),
                    Monitor = table.Column<string>(type: "TEXT", nullable: false),
                    DomainId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamingComputerDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamingComputerDetails_GamingComputers_GamingComputerId",
                        column: x => x.GamingComputerId,
                        principalTable: "GamingComputers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamingComputerDetails_GamingComputerId",
                table: "GamingComputerDetails",
                column: "GamingComputerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GamingSessions_ComputerId",
                table: "GamingSessions",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_GamingSessions_PlayerId",
                table: "GamingSessions",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Email",
                table: "Players",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamingComputerDetails");

            migrationBuilder.DropTable(
                name: "GamingSessions");

            migrationBuilder.DropTable(
                name: "OfficeComputers");

            migrationBuilder.DropTable(
                name: "GamingComputers");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Computers");
        }
    }
}
