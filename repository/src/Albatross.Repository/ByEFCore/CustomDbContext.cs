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
	public abstract class CustomDbContext : DbContext, IDbSession {
		IEnumerable<IBuildEntityModel> builders;
		public IDbConnection DbConnection { get; private set; }

		public CustomDbContext(IEnumerable<IBuildEntityModel> builders) {
			this.builders = builders;
		}

		public abstract IDbConnection CreateConnection(DbContextOptionsBuilder optionsBuilder);

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			this.DbConnection = CreateConnection(optionsBuilder);
			optionsBuilder.UseLazyLoadingProxies(false);
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			optionsBuilder.EnableDetailedErrors(true);
			optionsBuilder.EnableSensitiveDataLogging();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			foreach (var builder in builders) {
				builder.Build(modelBuilder);
			}
		}

		public void Migrate() {
			this.Database.Migrate();
		}

		public override void Dispose() {
			base.Dispose();
			if (this.DbConnection != null) {
				this.DbConnection.Dispose();
				this.DbConnection = null;
			}
		}

		public ITransaction BeginTransaction() {
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

		public string GetCreateScript() {
			return this.Database.GenerateCreateScript();
		}
	}
}