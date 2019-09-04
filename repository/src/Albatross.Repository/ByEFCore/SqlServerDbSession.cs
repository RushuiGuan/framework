using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Repository.ByEFCore {
	public class SqlServerDbSession : DbSession {
		string connectionString;

		public SqlServerDbSession(string connectionString, IEnumerable<IBuildEntityModel> builders):base(builders) {
			this.connectionString = connectionString;
		}

		public override IDbConnection CreateConnection(DbContextOptionsBuilder optionsBuilder) {
			SqlConnection conn = new SqlConnection(connectionString);
			conn.Open();
			optionsBuilder.UseSqlServer(conn);
			return conn;
		}


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			CreateConnection(optionsBuilder);

			optionsBuilder.UseLazyLoadingProxies(false);
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			optionsBuilder.EnableDetailedErrors(true);
            optionsBuilder.EnableSensitiveDataLogging();
		}
	}
}