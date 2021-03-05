using System.Linq;
using Albatross.Database.SqlServer;
using Xunit;

namespace Albatross.Database.UnitTest
{
	public class GeneralUnitTest {
		GetDbConnection GetDbConnection { get; } = new GetDbConnection(new GetConnectionString());

		public Database MasterDb { get; private set; } = new Database {
			DataSource = ".",
			InitialCatalog = "master",
			SSPI = true,
		};
		public Database AlbatrossDb { get; private set; } = new Database {
			DataSource = ".",
			InitialCatalog = "albatross",
			SSPI = true,
		};


		[Theory]
		[InlineData("localhost", "content", true, null, null, "Data Source=localhost;Initial Catalog=content;Integrated Security=True")]
		[InlineData("localhost", "content", false, "jdoe", "welcome", "Data Source=localhost;Initial Catalog=content;User ID=jdoe;Password=welcome")]
		public void GetConnectionStringTest(string server, string database, bool sspi, string username, string pwd, string expected) {
			string data = new GetConnectionString().Get(new Database {
				DataSource = server,
				InitialCatalog = database,
				SSPI = sspi,
				UserName = username,
				Password = pwd,
			});
			Assert.Equal(expected, data);
		}

		[Theory]
		[InlineData("int", "int")]
		[InlineData("bigint", "bigint")]
		public void GetSqlTypeTest(string name, string expected) {
			GetSqlType getSqlType = new GetSqlType(GetDbConnection);
			var type = getSqlType.Get(MasterDb, null, name);
			Assert.Equal(expected, type.Name);
		}

		[Fact]
		public void ListSqlTypeTest() {
			ListSqlType listSqlType = new ListSqlType(GetDbConnection);
			var types = listSqlType.List(MasterDb);
			Assert.NotEmpty(types);
		}

		[Theory]
		[InlineData("dyn", "svc", "Svc")]
		public void GetTableTest(string schema, string name, string expected) {
			GetTable getTable = new GetTable(GetDbConnection, new ListTableColumn(GetDbConnection, new GetTableColumnType(GetDbConnection)), new ListTableIndex(GetDbConnection, new ListTableIndexColumn(GetDbConnection)));
			var type = getTable.Get(AlbatrossDb, schema, name);
			Assert.Equal(expected, type.Name);
		}

		[Theory]
		[InlineData("dbo", "trade")]
		public void GetTableIndexTest(string schema, string name) {
			Table table = new Table {
				Database = AlbatrossDb,
				Schema = schema,
				Name = name,
			};
			ListTableIndex getTableIndex = new ListTableIndex(GetDbConnection, new ListTableIndexColumn(GetDbConnection));
			var indexes = getTableIndex.List(table);
			Assert.NotEmpty(indexes);
			foreach (var item in indexes) {
				Assert.NotEmpty(item.Columns);
			}
		}

		[Theory]
		[InlineData("dyn", "SetServiceType")]
		[InlineData("dyn", "SetSvcReferenceArray")]
		public void ListProcedureParameterTest(string schema, string name) {
			Procedure procedure = new Procedure {
				Database = AlbatrossDb,
				Schema = schema,
				Name = name,
			};
			var @params = new ListProcedureParameter(this.GetDbConnection).List(procedure);
			Assert.NotEmpty(@params);
		}

		[Theory]
		[InlineData("dyn", "SetServiceType")]
		[InlineData("dyn", "SetSvcReferenceArray")]
		public void GetProcedureTest(string schema, string name) {
			Procedure procedure = new Procedure {
				Database = AlbatrossDb,
				Schema = schema,
				Name = name,
			};
			var sp = new GetProcedure(this.GetDbConnection, new ListProcedureParameter(GetDbConnection), new GetProcedureDefinition(this.GetDbConnection), new GetDatabasePermission(this.GetDbConnection)).Get(AlbatrossDb, schema, name);
			Assert.Equal(sp.Name, name);
			Assert.Equal(sp.Schema, schema);
			Assert.NotEmpty(sp.Parameters);
		}

		[Theory]
		[InlineData("ac", "createcompany", "")]
		public void GetProcedureDefinitionTest(string schema, string name, string expected) {
			Procedure procedure = new Procedure() {
				Schema = schema,
				Name = name,
				Database = new Database {
					DataSource = ".",
					InitialCatalog = "ac",
					SSPI = true,
				}
			};
			string result = new GetProcedureDefinition(GetDbConnection).Get(procedure);
			Assert.Equal(expected, result);
		}
	}
}
