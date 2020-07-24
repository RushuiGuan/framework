using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Repository.Core
{
	/// <summary>
	/// Represent a database session.  Has a open database connection and should be disposed when done
	/// </summary>
	public interface IDbSession : IDisposable {
		IDbConnection DbConnection { get; }
		DbContext DbContext { get; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		ITransaction BeginTransaction();
		string GetCreateScript();
		void EnsureCreated();
		bool IsChanged(object t);
	}
}
