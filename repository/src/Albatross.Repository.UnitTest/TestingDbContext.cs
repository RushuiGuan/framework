using System.Collections.Generic;
using Albatross.Config.Core;
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Albatross.Repository.UnitTest {
	public class TestingDbContext : Albatross.Repository.NUnit.SqlLiteInMemoryDbContext {
		public TestingDbContext(IModelBuilderFactory factory) : base(factory.Get(typeof(TestingDbContext).Assembly)) { }
	}
}