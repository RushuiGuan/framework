using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.EFCore.Admin.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class SampleSqlServerMigration_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sam");

            migrationBuilder.CreateTable(
                name: "EntityInADiffNameSpace",
                schema: "sam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityInADiffNameSpace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JsonData",
                schema: "sam",
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
                name: "Market",
                schema: "sam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Market", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractSpec",
                schema: "sam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarketId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractSpec", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ContractSpec_Market_MarketId",
                        column: x => x.MarketId,
                        principalSchema: "sam",
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpreadSpec",
                schema: "sam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarketId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpreadSpec", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_SpreadSpec_Market_MarketId",
                        column: x => x.MarketId,
                        principalSchema: "sam",
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractSpec_MarketId_StartDate",
                schema: "sam",
                table: "ContractSpec",
                columns: new[] { "MarketId", "StartDate" },
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_SpreadSpec_MarketId_StartDate",
                schema: "sam",
                table: "SpreadSpec",
                columns: new[] { "MarketId", "StartDate" },
                unique: true)
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractSpec",
                schema: "sam");

            migrationBuilder.DropTable(
                name: "EntityInADiffNameSpace",
                schema: "sam");

            migrationBuilder.DropTable(
                name: "JsonData",
                schema: "sam");

            migrationBuilder.DropTable(
                name: "SpreadSpec",
                schema: "sam");

            migrationBuilder.DropTable(
                name: "Market",
                schema: "sam");
        }
    }
}
