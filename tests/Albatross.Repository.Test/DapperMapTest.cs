using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Repository.Test
{
	public class Data{
		public int Id { get; set; }
		public string Name { get; set; } 

		public Data(int id, string name) {
			this.Name = name;
			this.Id = id;
		}
	}
	public class DapperMapTest {
		[Fact]
		public async Task TestMapping() {
			var sql = "select top 1 Id, Name from data;";
			using (var conn = new SqlConnection("Server=.;Database=test;Trusted_Connection=true")) {
				var items = await conn.QueryAsync<Data>(sql);
				Assert.Single(items);
				Assert.NotEmpty(items.First().Name);
			}
		}
	}
}
