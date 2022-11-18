using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Repository.ByEFCore {
	public class EFCoreTransaction : ITransaction {
		IDbContextTransaction dbContextTransaction;
		public EFCoreTransaction(IDbContextTransaction dbContextTransaction) {
			this.dbContextTransaction = dbContextTransaction;
		}

		public IDbTransaction DbTransaction => this.dbContextTransaction.GetDbTransaction();

		public void Commit() {
			this.dbContextTransaction.Commit();
		}


		public void Dispose() {
			this.dbContextTransaction.Dispose();
		}

		public void Rollback() {
			this.dbContextTransaction.Rollback();
		}
	}
}
