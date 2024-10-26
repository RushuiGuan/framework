using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.EFCore {
	/// <summary>
	/// Represent a database session.  Has a open database connection and should be disposed when done
	/// </summary>
	public interface IDbSession : IDisposable {
		IDbConnection DbConnection { get; }
		DbContext DbContext { get; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		int SaveChanges();
	}
	public static class DbSessionExtensions {
		public static void EnsureCreated(this IDbSession session) => session.DbContext.Database.EnsureCreated();
		public static string GetCreateScript(this IDbSession session) => session.DbContext.Database.GenerateCreateScript();
		public static IDbContextTransaction BeginTransaction(this IDbSession session) => session.DbContext.Database.BeginTransaction();
		public static Task<IDbContextTransaction> BeginTransactionAsync(this IDbSession session, CancellationToken cancellationToken) => session.DbContext.Database.BeginTransactionAsync(cancellationToken);
	}
}