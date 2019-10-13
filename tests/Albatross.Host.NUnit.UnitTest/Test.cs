using Albatross.Host.NUnit;
using NUnit.Framework;
using System;

namespace Albatross.Host.NUnit.UnitTest {
	[TestFixture]
	public class BootStrapTest: TestBase<TestUnitOfWork> {

		[Test]
		public void Run() {
			Assert.NotNull(NewUnitOfWork());
		}
	}
}
