using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Albatross.Repository.Test.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class MySqlServerMigrationv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "test");

            migrationBuilder.CreateTable(
                name: "FutureMarket",
                schema: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ContractSize = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FutureMarket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JsonData",
                schema: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rule = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JsonData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TickSize",
                schema: "test",
                columns: table => new
                {
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MarketId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickSize", x => new { x.MarketId, x.StartDate })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_TickSize_FutureMarket_MarketId",
                        column: x => x.MarketId,
                        principalSchema: "test",
                        principalTable: "FutureMarket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TickSize_MarketId_StartDate_EndDate",
                schema: "test",
                table: "TickSize",
                columns: new[] { "MarketId", "StartDate", "EndDate" })
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JsonData",
                schema: "test");

            migrationBuilder.DropTable(
                name: "TickSize",
                schema: "test");

            migrationBuilder.DropTable(
                name: "FutureMarket",
                schema: "test");
        }
    }
}
