using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;

namespace Albatross.Repository.ByEFCore {
	public abstract class DbSession : DbContext, IDbSession {
		public DbContext DbContext => this;
		public IDbConnection DbConnection => this.Database.GetDbConnection();

		public DbSession(DbContextOptions option) :base(option){
		}

		public virtual Assembly EntityModelAssembly => this.GetType().Assembly;

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			var items = EntityModelAssembly.GetEntityModels();
			foreach (var item in items) {
				item.Build(modelBuilder);
			}
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
			try {
				return base.SaveChangesAsync(cancellationToken);
			} catch (DbUpdateException err) when (err.TryConvertError(out Exception converted)) {
				throw converted;
			}
		}
		public string GetCreateScript() =>this.Database.GenerateCreateScript();
		public void EnsureCreated() => this.Database.EnsureCreated();
		public ITransaction BeginTransaction() => new EFCoreTransaction(this.Database.BeginTransaction());
	}
}