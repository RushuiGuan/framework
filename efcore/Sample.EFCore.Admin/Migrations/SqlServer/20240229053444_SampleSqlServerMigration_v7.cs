using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.EFCore.Admin.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class SampleSqlServerMigration_v7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UtcTimeStamp",
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
                name: "UtcTimeStamp",
                schema: "sam",
                table: "MyData");
        }
    }
}
