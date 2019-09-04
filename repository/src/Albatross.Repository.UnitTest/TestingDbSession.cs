using System.Collections.Generic;
using Albatross.Config.Core;
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Albatross.Repository.UnitTest {
	public class TestingDbSession : Albatross.Repository.NUnit.SqlLiteInMemoryDbSession {
		public TestingDbSession(IModelBuilderFactory factory) : base(factory.Get(typeof(TestingDbSession).Assembly)) { }
	}
}