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
	public abstract class DbSession : DbContext, IDbSession {
		IEnumerable<IBuildEntityModel> builders;
		public IDbConnection DbConnection { get; private set; }

		public DbContext DbContext => this;

		public DbSession(IEnumerable<IBuildEntityModel> builders) {
			this.builders = builders;
		}

		public abstract IDbConnection CreateConnection(DbContextOptionsBuilder optionsBuilder);

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			this.DbConnection = CreateConnection(optionsBuilder);
			optionsBuilder.UseLazyLoadingProxies(true);
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			optionsBuilder.EnableDetailedErrors(true);
			optionsBuilder.EnableSensitiveDataLogging();
		}


		public override void Dispose() {
			base.Dispose();
			if (this.DbConnection != null) {
				this.DbConnection.Dispose();
				this.DbConnection = null;
			}
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
			try {
				return base.SaveChangesAsync(cancellationToken);
			} catch (DbUpdateException err) when (err.TryConvertError(out Exception converted)) {
				throw converted;
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) => builders.ForEach(args => args.Build(modelBuilder));
		public string GetCreateScript() =>this.Database.GenerateCreateScript();
		public void EnsureCreated() => this.Database.EnsureCreated();
		public ITransaction BeginTransaction() => new EFCoreTransaction(this.Database.BeginTransaction());
	}
}