using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Albatross.Repository.ByEFCore {
	public class SqlServerCreateScriptDbSession : DbSession {
		public SqlServerCreateScriptDbSession(IEnumerable<IBuildEntityModel> builders) : base(builders) {
		}

		public override IDbConnection CreateConnection(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseSqlServer("Server=LocalHost");
			return null;
		}
	}
}
