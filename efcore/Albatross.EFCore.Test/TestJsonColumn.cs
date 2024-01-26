using Albatross.Hosting.Test;
using Microsoft.EntityFrameworkCore;
using Sample.EFCore;
using Sample.EFCore.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestJsonColumn : IClassFixture<MyTestHost> {
		private readonly MyTestHost host;

		public TestJsonColumn(MyTestHost host) {
			this.host = host;
		}

		async Task Create(SampleDbSession session, string text) {
			var set = session.Set<MyData>();
			var data = new MyData();
			data.Property.Text = text;
			set.Add(data);
			await session.SaveChangesAsync();
		}
		// [Fact(Skip ="require sql server")]
		[Fact]
		public async Task TestWrite() {
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();

			var set = session.Set<MyData>();
			set.Add(new MyData());
			await session.SaveChangesAsync();
		}

		//[Fact(Skip ="require sql server")]
		[Fact]
		public async Task TestRead() {
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			await session.Set<MyData>().ExecuteDeleteAsync();
			var text = "test";
			await Create(session, text);
			var items = await session.Set<MyData>().ToArrayAsync();
			Assert.NotEmpty(items);
			Assert.NotNull(items.First().Property);
			Assert.Equal(text, items.First().Property.Text);
		}
	}
}
