using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Albatross.CodeGen.UnitTest {
	public class TestBase {
		protected ServiceProvider provider;

		[OneTimeSetUp]
		public void OneTimeSetUp() {
			ServiceCollection service = new ServiceCollection();
			service.AddDefaultCodeGen();
			provider = service.BuildServiceProvider();
		}
	}
}
