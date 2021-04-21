using Polly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestOperation : IClassFixture<MyTestHost> {
		private readonly MyTestHost host;

		public TestOperation(MyTestHost host) {
			this.host = host;
		}

		[Fact]
		public async Task TestNormalOperation() {
			var scope = host.Create();
			var factory = scope.Get<ICacheManagementFactory>();
			var cacheMgmt = factory.Get<string>(nameof(MyCacheMgmt));
			string key = "a";
			int count = 0;
			var t1 = await cacheMgmt.ExecuteAsync(async context => {
				await Task.Delay(1000);
				count++;
				return "dingle";
			}, new Context(key));


			var t2 = await cacheMgmt.ExecuteAsync(async context => {
				await Task.Delay(1000);
				count++;
				return "dingle";
			}, new Context(key));

			var t3 = await cacheMgmt.ExecuteAsync(async context => {
				await Task.Delay(1000);
				count++;
				return "dingle";
			}, new Context(key));

			var t4 = await cacheMgmt.ExecuteAsync(async context => {
				await Task.Delay(1000);
				count++;
				return "dingle";
			}, new Context(key));

			// await Task.WhenAll(new[] { t1, t2, });
			Assert.Equal(1, count);
		}
	}
}