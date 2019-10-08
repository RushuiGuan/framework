using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Albatross.Repository.ByEFCore {
	public abstract class DbSession : DbContext, IDbSession {
		IEnumerable<IBuildEntityModel> builders;

		public DbContext DbContext => this;

		public IDbConnection DbConnection => this.Database.GetDbConnection();

		public DbSession(DbContextOptions option, IEnumerable<IBuildEntityModel> builders) :base(option){
			this.builders = builders;
		}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
		//	this.DbConnection = CreateConnection(optionsBuilder);
		//	optionsBuilder.UseLazyLoadingProxies(false);
		//	optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
		//	optionsBuilder.EnableDetailedErrors(true);
		//	optionsBuilder.EnableSensitiveDataLogging();
		//}


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