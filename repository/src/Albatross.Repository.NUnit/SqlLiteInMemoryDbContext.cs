using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;

namespace Albatross.Repository.NUnit {
	public class SqlLiteInMemoryDbContext : CustomDbContext {
		const string ConnectionString = "Data Source=:memory:";
		public SqlLiteInMemoryDbContext(IEnumerable<IBuildEntityModel> builders) : base(builders) {
		}

		public override IDbConnection CreateConnection(DbContextOptionsBuilder optionsBuilder) {
			var conn = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
			conn.Open();
			optionsBuilder.UseSqlite(conn);
			return conn;
		}
	}
}
