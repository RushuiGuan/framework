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
	public class DbContextSession : DbContext, IDbSession {
		IEnumerable<IBuildEntityModel> builders;
		string connectionString;

		public IDbConnection DbConnection { get; private set; }

		public DbContextSession(string connectionString, IEnumerable<IBuildEntityModel> builders) {
			this.connectionString = connectionString;
			this.builders = builders;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			string connectionString = this.connectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            DbConnection = conn;
            DbConnection.Open();
			optionsBuilder.UseLazyLoadingProxies(false);
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			optionsBuilder.UseSqlServer(conn);
			optionsBuilder.EnableDetailedErrors(true);
            optionsBuilder.EnableSensitiveDataLogging();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			foreach (var builder in builders) {
                builder.Build(modelBuilder);
			}
		}

		public override void Dispose() {
            base.Dispose();
            this.DbConnection?.Dispose();
		}

		public void Migrate() {
			this.Database.Migrate();
		}

        public ITransaction BeginTransaction()
        {
            IDbContextTransaction t = this.Database.BeginTransaction();
            return new EFCoreTransaction(t);
        }

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
			try {
				return base.SaveChangesAsync(cancellationToken);
			} catch (DbUpdateException err) when (err.TryConvertError(out Exception converted)) {
				throw converted;
			}
		}
    }
}