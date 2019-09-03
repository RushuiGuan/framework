using Albatross.Host.NUnit;
using Albatross.Repository.ByEFCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.NUnit {
	public class InMemoryDbUnitOfWork<T> : TestUnitOfWork where T : CustomDbContext {
		public InMemoryDbUnitOfWork(IServiceScope scope) :base(scope){
			Get<T>().Database.EnsureCreated();
		}
	}
}
