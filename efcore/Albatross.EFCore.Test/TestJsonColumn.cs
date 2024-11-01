using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Sample.EFCore;
using Sample.EFCore.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestJsonColumn {
		// [Fact(Skip ="require sql server")]
		[Fact]
		public async Task TestWriteJsonColumn() {
			using var host = My.Create();
			using var scope = host.Services.CreateScope();
			var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();

			var set = session.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();

			Assert.NotEqual(0, data.Id);
			data.Property = new JsonProperty(DateTime.Now.Ticks.ToString());
			Assert.Equal(EntityState.Modified, session.DbContext.Entry(data).State);

			Assert.NotEqual(0, data.Id);
			data.Property = new JsonProperty(null);
			// the state will remain modified, even though the value is the same.  See github issue below:
			// https://github.com/dotnet/efcore/issues/13367
			Assert.Equal(EntityState.Modified, session.DbContext.Entry(data).State);
		}

		[Fact]
		public async Task TestWriteArrayColumn() {
			using var host = My.Create();
			using var scope = host.Services.CreateScope();
			var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();

			var set = session.Set<MyData>();
			var data = new MyData();
			set.Add(data);
			await session.SaveChangesAsync();
			Assert.NotEqual(0, data.Id);
			data.ArrayProperty.Add(new JsonProperty("test"));
			await session.SaveChangesAsync();
			data.ArrayProperty.Add(new JsonProperty("test"));
			Assert.Equal(2, data.ArrayProperty.Count);
			Assert.Equal(EntityState.Modified, session.DbContext.Entry(data).State);
			await session.SaveChangesAsync();
		}

		//[Fact(Skip ="require sql server")]
		[Fact]
		public async Task TestRead() {
			using var host = My.Create();
			using var scope = host.Services.CreateScope();
			int id;
			var text = "test";
			using (var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>()) {
				var data = new MyData();
				data.Property = new JsonProperty(text);
				session.Set<MyData>().Add(data);
				await session.SaveChangesAsync();
				id = data.Id;
			}

			using (var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>()) {
				var item = await session.Set<MyData>().FirstOrDefaultAsync(p => p.Id == id);
				Assert.NotNull(item);
				Assert.NotNull(item.Property);
				Assert.Equal(text, item.Property.Text);
			}
		}
	}
}