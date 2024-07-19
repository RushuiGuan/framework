using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.EFCore.Admin.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class SampleSqlServerMigration_v9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "sam",
                table: "Data3",
                type: "varchar(900)",
                unicode: false,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "sam",
                table: "Data2",
                type: "varchar(900)",
                unicode: false,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "sam",
                table: "Data1",
                type: "varchar(900)",
                unicode: false,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Data3_Name",
                schema: "sam",
                table: "Data3",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Data2_Name",
                schema: "sam",
                table: "Data2",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Data1_Name",
                schema: "sam",
                table: "Data1",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Data3_Name",
                schema: "sam",
                table: "Data3");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Data2_Name",
                schema: "sam",
                table: "Data2");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Data1_Name",
                schema: "sam",
                table: "Data1");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "sam",
                table: "Data3");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "sam",
                table: "Data2");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "sam",
                table: "Data1");
        }
    }
}
