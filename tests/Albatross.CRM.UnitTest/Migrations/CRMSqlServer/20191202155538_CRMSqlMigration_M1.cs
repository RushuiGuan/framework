using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Albatross.CRM.UnitTest.Migrations.CRMSqlServer
{
    public partial class CRMSqlMigration_M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "crm");

            migrationBuilder.CreateSequence(
                name: "Hilo",
                schema: "crm");

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "crm",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Company = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "crm",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(maxLength: 2046, nullable: false),
                    AvailableDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                    table.UniqueConstraint("AK_Product_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                schema: "crm",
                columns: table => new
                {
                    ContactID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    CustomerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactID);
                    table.UniqueConstraint("AK_Contact_Name", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Contact_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalSchema: "crm",
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "License",
                schema: "crm",
                columns: table => new
                {
                    LicenseID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    BeginDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_License", x => x.LicenseID);
                    table.ForeignKey(
                        name: "FK_License_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalSchema: "crm",
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_License_Product_ProductID",
                        column: x => x.ProductID,
                        principalSchema: "crm",
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "crm",
                columns: table => new
                {
                    AddressID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(maxLength: 128, nullable: true),
                    Street = table.Column<string>(maxLength: 256, nullable: true),
                    City = table.Column<string>(maxLength: 256, nullable: true),
                    State = table.Column<string>(maxLength: 128, nullable: true),
                    ContactID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_Address_Contact_ContactID",
                        column: x => x.ContactID,
                        principalSchema: "crm",
                        principalTable: "Contact",
                        principalColumn: "ContactID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_ContactID",
                schema: "crm",
                table: "Address",
                column: "ContactID");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CustomerID",
                schema: "crm",
                table: "Contact",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Name",
                schema: "crm",
                table: "Customer",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_License_CustomerID",
                schema: "crm",
                table: "License",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_License_ProductID",
                schema: "crm",
                table: "License",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "License",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "Contact",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "crm");

            migrationBuilder.DropSequence(
                name: "Hilo",
                schema: "crm");
        }
    }
}
