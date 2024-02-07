using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.EFCore.Admin.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class SampleSqlServerMigration_v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Text",
                schema: "sam",
                table: "MyData",
                type: "varchar(max)",
                unicode: false,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                schema: "sam",
                table: "MyData");
        }
    }
}
