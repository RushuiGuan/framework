using Albatross.Host.NUnit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.UnitTest {
	public class SqliteUnitOfWork : TestUnitOfWork {
		public SqliteUnitOfWork(IServiceScope scope) : base(scope) {
			Get<CRMDbSession>().EnsureCreated();
		}
	}
}
