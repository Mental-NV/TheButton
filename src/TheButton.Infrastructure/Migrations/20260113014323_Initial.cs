using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheButton.Infrastructure.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "write");

            migrationBuilder.EnsureSchema(
                name: "read");

            migrationBuilder.CreateTable(
                name: "Commands",
                schema: "write",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdempotencyKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResultJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "write",
                columns: table => new
                {
                    Position = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OccurredUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserVersion = table.Column<long>(type: "bigint", nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Position);
                });

            migrationBuilder.CreateTable(
                name: "UserCounters",
                schema: "read",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCounters", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commands_Operation_UserId_IdempotencyKey",
                schema: "write",
                table: "Commands",
                columns: new[] { "Operation", "UserId", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventType_Position",
                schema: "write",
                table: "Events",
                columns: new[] { "EventType", "Position" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId_UserVersion",
                schema: "write",
                table: "Events",
                columns: new[] { "UserId", "UserVersion" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.Sql("CREATE VIEW [read].[GlobalCounter] AS SELECT ISNULL(MAX(Position), 0) AS GlobalValue FROM [write].[Events] WHERE EventType = 'CounterIncremented';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commands",
                schema: "write");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "write");

            migrationBuilder.DropTable(
                name: "UserCounters",
                schema: "read");

            migrationBuilder.Sql("DROP VIEW IF EXISTS [read].[GlobalCounter];");
        }
    }
}
