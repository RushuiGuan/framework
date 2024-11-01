using Albatross.DateLevel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.EFCore;
using Sample.EFCore.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test.MyNamespace {
	public class TestSampleDbSession  {
		[Fact]
		public void TestGetEntityModels() {
			var items = typeof(Market).Assembly.GetEntityModels(null);
			Assert.NotEmpty(items);

			items = typeof(Market).Assembly.GetEntityModels("Sample.EFCore.Models.Test");
			Assert.Single(items);
		}

		//[Fact(Skip = "require database")]
		[Fact]
		public async Task TestContractSpecPersistance() {
			string marketName = "test";
			using var host = My.Create();
			using var scope = host.Services.CreateScope();
			var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
			var set = session.DbContext.Set<Market>();
			var market = await set
				.Include(args => args.ContractSpec)
				.Where(args => args.Name == marketName).FirstOrDefaultAsync();
			if (market == null) {
				market = new Market(marketName);
				set.Add(market);
				await session.SaveChangesAsync();
			}

			DateOnly startDate = new DateOnly(1980, 1, 1);
			market.ContractSpec.SetDateLevel<ContractSpec, int>(new ContractSpec(market.Id, startDate, 1));
			market.ContractSpec.SetDateLevel<ContractSpec, int>(new ContractSpec(market.Id, new DateOnly(1980, 2, 1), 2));
			market.ContractSpec.SetDateLevel<ContractSpec, int>(new ContractSpec(market.Id, new DateOnly(1980, 3, 1), 3));
			market.ContractSpec.SetDateLevel<ContractSpec, int>(new ContractSpec(market.Id, new DateOnly(1980, 4, 1), 3));
			await session.SaveChangesAsync();
			market.ContractSpec.SetDateLevel<ContractSpec, int>(new ContractSpec(market.Id, new DateOnly(1980, 3, 1), 2));
			await session.SaveChangesAsync();
			market.ContractSpec.Clear();
			await session.SaveChangesAsync();
		}

		//[Fact(Skip = "require database")]
		[Fact]
		public async Task TestSpreadSpecPersistance() {
			string marketName = "test";
			using var host = My.Create();
			using var scope = host.Services.CreateScope();
			var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
			var set = session.DbContext.Set<Market>();
			var market = set
				.Include(args => args.SpreadSpec)
				.Where(args => args.Name == marketName).FirstOrDefault();

			if (market == null) {
				market = new Market(marketName);
				set.Add(market);
				await session.SaveChangesAsync();
			}

			DateOnly startDate = new DateOnly(1980, 1, 1);
			market.SpreadSpec.SetDateLevel<SpreadSpec, int>(new SpreadSpec(market.Id, startDate, 1));
			market.SpreadSpec.SetDateLevel<SpreadSpec, int>(new SpreadSpec(market.Id, new DateOnly(1980, 2, 1), 2));
			market.SpreadSpec.SetDateLevel<SpreadSpec, int>(new SpreadSpec(market.Id, new DateOnly(1980, 3, 1), 3));
			market.SpreadSpec.SetDateLevel<SpreadSpec, int>(new SpreadSpec(market.Id, new DateOnly(1980, 4, 1), 3));
			await session.SaveChangesAsync();

			market.SpreadSpec.SetDateLevel<SpreadSpec, int>(new SpreadSpec(market.Id, new DateOnly(1980, 3, 1), 2));
			await session.SaveChangesAsync();

			market.SpreadSpec.Clear();
			await session.SaveChangesAsync();
		}

		[Fact]
		public async Task TestChangeTracker() {
			using var host = My.Create();
			using (var scope = host.Services.CreateScope()) {
				var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
				var set = session.DbContext.Set<MyData>();
				for (int i = 0; i < 10; i++) {
					var data = new MyData { Int = i };
					set.Add(data);
				}
				await session.SaveChangesAsync();
			}
			using (var scope = host.Services.CreateScope()) {
				var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
				var array = await session.DbContext.Set<MyData>().ToArrayAsync();
				Assert.Equal(array.Length, session.ChangeTracker.Entries().Count());
			}
			//using (var scope = host.Create()) {
			//	var session = scope.ServiceProvider.GetRequiredService<SampleDbSession>();
			//	var array = await session.DbContext.Set<MyData>().ToArrayAsync();
			//	array[0].Int = 100;
			//	Assert.Empty(session.ChangeTracker.Entries());
			//}
		}
	}
}