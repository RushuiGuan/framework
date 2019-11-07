using Microsoft.EntityFrameworkCore.Migrations;

namespace Albatross.CRM.UnitTest.Migrations.CRMSqlite
{
    public partial class CRMSqliteMigration_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                schema: "crm",
                table: "Customer",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                schema: "crm",
                table: "Customer");
        }
    }
}
