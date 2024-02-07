using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.EFCore.Admin.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class SampleSqlServerMigration_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArrayProperty",
                schema: "sam",
                table: "MyData",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrayProperty",
                schema: "sam",
                table: "MyData");
        }
    }
}
