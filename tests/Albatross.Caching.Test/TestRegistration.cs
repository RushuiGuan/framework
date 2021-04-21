using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.Caching.Test {

	public class TestRegistration : IClassFixture<MyTestHost>{
		private readonly MyTestHost host;

		public TestRegistration(MyTestHost host) {
			this.host = host;
		}

		[Fact]
		public void TestMultipleRegistrations() {
			var scope = host.Create();
			var items = scope.Get<IEnumerable<ICacheManagement>>();
			Assert.Equal(2, items.Count());
		}

		[Fact]
		public void TestSingletonRegistrationAgainstCollectionRegistration() {
			var scope = host.Create();
			var items = scope.Get<IEnumerable<ICacheManagement>>();
			var single = scope.Get<ICacheManagement>();
			Assert.NotSame(single, items.First());
			Assert.Same(single, items.Last());
		}
	}
}
