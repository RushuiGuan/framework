using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.EFCore.Admin.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class SampleSqlServerMigration_v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Bool",
                schema: "sam",
                table: "MyData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "sam",
                table: "MyData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                schema: "sam",
                table: "MyData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                schema: "sam",
                table: "MyData",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                schema: "sam",
                table: "MyData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Decimal",
                schema: "sam",
                table: "MyData",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "Double",
                schema: "sam",
                table: "MyData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "Float",
                schema: "sam",
                table: "MyData",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "sam",
                table: "MyData",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Int",
                schema: "sam",
                table: "MyData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "sam",
                table: "MyData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedUtc",
                schema: "sam",
                table: "MyData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bool",
                schema: "sam",
                table: "MyData");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "sam",
                table: "MyData");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                schema: "sam",
                table: "MyData");

            migrationBuilder.DropColumn(
                name: "Date",
                schema: "sam",
                table: "MyData");

            migrationBuilder.DropColumn(
                name: "DateTime",
                schema: "sam",
                table: "MyData");

            migrationBuilder.DropColumn(
                name: "Decimal",
                schema: "sam",
                table: "MyData");

            migrationBuilder.DropColumn(
                name: "Double",
                schema: "sam",
                table: "MyData");

            migrationBuilder.DropColumn(
                name: "Float",
                schema: "sam",
                table: "MyData");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "sam",
                table: "MyData");

            migrationBuilder.DropColumn(
                name: "Int",
                schema: "sam",
                table: "MyData");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "sam",
                table: "MyData");

            migrationBuilder.DropColumn(
                name: "ModifiedUtc",
                schema: "sam",
                table: "MyData");
        }
    }
}
