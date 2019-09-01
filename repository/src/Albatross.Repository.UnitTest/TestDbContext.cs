using System.Collections.Generic;
using Albatross.Config.Core;
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Albatross.Repository.UnitTest {
	public class TestDbContext : ByEFCore.DbContextSession {
		public TestDbContext(GetTestDatabaseConnectionString getConnectionString, IModelBuilderFactory factory) : base(getConnectionString.Get(), factory.Get(typeof(TestDbContext).Assembly)) { }
	}
}