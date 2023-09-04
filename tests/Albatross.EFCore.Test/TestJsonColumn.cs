using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestJsonColumn : IClassFixture<MyTestHost> {
		private readonly MyTestHost host;

		public TestJsonColumn(MyTestHost host) {
			this.host = host;
		}

		async Task Create(MyDbSession session) {
			var set = session.Set<JsonData>();
			set.Add(new JsonData {
				Rule = new MyJsonData("c", "vwap", "ms") {
					Duration = 60,
				}
			});
			set.Add(new JsonData {
				Rule = new HisJsonData("a", "ic", "g") {
					ExecutionDuration = 1,
					HasExecutionWindow = true,
					UseCashClose = false,
				}
			});
			await session.SaveChangesAsync();
		}
		[Fact(Skip ="require sql server")]
		public async Task TestWrite() {
			using var scope = host.Create();
			var session = scope.Get<MyDbSession>();

			var set = session.Set<JsonData>();
			set.Add(new JsonData { 
				Rule = new MyJsonData("c", "vwap", "ms") {
					Duration = 60,
				}
			});
			set.Add(new JsonData {
				Rule = new HisJsonData("a", "ic", "g") {
					 ExecutionDuration = 1,
					 HasExecutionWindow = true,
					 UseCashClose = false,
				}
			});
			await session.SaveChangesAsync();
		}

		[Fact(Skip ="require sql server")]
		public async Task TestRead() {
			using var scope = host.Create();
			var session = scope.Get<MyDbSession>();
			await Create(session);
			var items = await session.Set<JsonData>().ToArrayAsync();
			foreach(var item in items) {
				Assert.False(item?.Rule is EmptyJsonData);
			}
		}
	}
}
