using Microsoft.Extensions.DependencyInjection;
using Sample.EFCore;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestAutoCacheEviction {
		[Fact]
		public async Task TestRun() {
			using var host = My.Create();
			using var scope = host.Services.CreateScope();
			var svc = scope.ServiceProvider.GetRequiredService<MyDataService>();
			var data = new MyData { Text = "test" };
			await svc.Set(data);
			Assert.Equal("test", data.Text);

			var cachedData1 = await svc.Get(data.Id);
			Assert.Equal("test", data.Text);

			data.Text = "test2";
			await svc.Set(data);

			var cachedData2 = await svc.Get(data.Id);
			Assert.Equal("test2", cachedData2.Text);
		}
	}
}