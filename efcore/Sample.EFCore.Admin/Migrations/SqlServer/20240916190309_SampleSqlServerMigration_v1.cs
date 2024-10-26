using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Sample.EFCore.Admin.Migrations.SqlServer {
	/// <inheritdoc />
	public partial class SampleSqlServerMigration_v1 : Migration {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.EnsureSchema(
				name: "sam");

			migrationBuilder.CreateTable(
				name: "Data2",
				schema: "sam",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1")
						.Annotation("SqlServer:IsTemporal", true)
						.Annotation("SqlServer:TemporalHistoryTableName", "Data2History")
						.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
						.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
						.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
					Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false)
						.Annotation("SqlServer:IsTemporal", true)
						.Annotation("SqlServer:TemporalHistoryTableName", "Data2History")
						.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
						.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
						.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
					Property = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
						.Annotation("SqlServer:IsTemporal", true)
						.Annotation("SqlServer:TemporalHistoryTableName", "Data2History")
						.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
						.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
						.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
					PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
						.Annotation("SqlServer:IsTemporal", true)
						.Annotation("SqlServer:TemporalHistoryTableName", "Data2History")
						.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
						.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
						.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
					PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
						.Annotation("SqlServer:IsTemporal", true)
						.Annotation("SqlServer:TemporalHistoryTableName", "Data2History")
						.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
						.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
						.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
				},
				constraints: table => {
					table.PrimaryKey("PK_Data2", x => x.Id);
					table.UniqueConstraint("AK_Data2_Name", x => x.Name);
				})
				.Annotation("SqlServer:IsTemporal", true)
				.Annotation("SqlServer:TemporalHistoryTableName", "Data2History")
				.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
				.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
				.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

			migrationBuilder.CreateTable(
				name: "Data3",
				schema: "sam",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1")
						.Annotation("SqlServer:IsTemporal", true)
						.Annotation("SqlServer:TemporalHistoryTableName", "Data3History")
						.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
						.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
						.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
					Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false)
						.Annotation("SqlServer:IsTemporal", true)
						.Annotation("SqlServer:TemporalHistoryTableName", "Data3History")
						.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
						.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
						.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
					ArrayProperty = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
						.Annotation("SqlServer:IsTemporal", true)
						.Annotation("SqlServer:TemporalHistoryTableName", "Data3History")
						.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
						.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
						.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
					PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
						.Annotation("SqlServer:IsTemporal", true)
						.Annotation("SqlServer:TemporalHistoryTableName", "Data3History")
						.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
						.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
						.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
					PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
						.Annotation("SqlServer:IsTemporal", true)
						.Annotation("SqlServer:TemporalHistoryTableName", "Data3History")
						.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
						.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
						.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
				},
				constraints: table => {
					table.PrimaryKey("PK_Data3", x => x.Id);
					table.UniqueConstraint("AK_Data3_Name", x => x.Name);
				})
				.Annotation("SqlServer:IsTemporal", true)
				.Annotation("SqlServer:TemporalHistoryTableName", "Data3History")
				.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
				.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
				.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

			migrationBuilder.CreateTable(
				name: "EntityInADiffNameSpace",
				schema: "sam",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1")
				},
				constraints: table => {
					table.PrimaryKey("PK_EntityInADiffNameSpace", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Market",
				schema: "sam",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_Market", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "MyData",
				schema: "sam",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Property = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
					ArrayProperty = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
					Text = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
					Date = table.Column<DateOnly>(type: "date", nullable: false),
					DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					UtcTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
					Int = table.Column<int>(type: "int", nullable: false),
					Decimal = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
					Bool = table.Column<bool>(type: "bit", nullable: false),
					Double = table.Column<double>(type: "float", nullable: false),
					Float = table.Column<float>(type: "real", nullable: false),
					Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ModifiedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
					ModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
					CreatedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
					CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_MyData", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "ContractSpec",
				schema: "sam",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					MarketId = table.Column<int>(type: "int", nullable: false),
					Value = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
					StartDate = table.Column<DateOnly>(type: "date", nullable: false),
					EndDate = table.Column<DateOnly>(type: "date", nullable: false)
				},
				constraints: table => {
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
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					MarketId = table.Column<int>(type: "int", nullable: false),
					Value = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
					StartDate = table.Column<DateOnly>(type: "date", nullable: false),
					EndDate = table.Column<DateOnly>(type: "date", nullable: false)
				},
				constraints: table => {
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
		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropTable(
				name: "ContractSpec",
				schema: "sam");

			migrationBuilder.DropTable(
				name: "Data2",
				schema: "sam")
				.Annotation("SqlServer:IsTemporal", true)
				.Annotation("SqlServer:TemporalHistoryTableName", "Data2History")
				.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
				.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
				.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

			migrationBuilder.DropTable(
				name: "Data3",
				schema: "sam")
				.Annotation("SqlServer:IsTemporal", true)
				.Annotation("SqlServer:TemporalHistoryTableName", "Data3History")
				.Annotation("SqlServer:TemporalHistoryTableSchema", "sam")
				.Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
				.Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

			migrationBuilder.DropTable(
				name: "EntityInADiffNameSpace",
				schema: "sam");

			migrationBuilder.DropTable(
				name: "MyData",
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