using Albatross.DateLevel;
using Albatross.Hosting.Test;
using Microsoft.EntityFrameworkCore;
using Sample.EFCore.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test.MyNamespace {
	public class TestSampleDbSession : IClassFixture<MyTestHost> {
		private readonly MyTestHost host;
		
		public TestSampleDbSession(MyTestHost host) {
			this.host = host;
		}

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
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
			var set = session.DbContext.Set<Market>();
			var market = set
				.Include(args => args.ContractSpec)
				.Where(args => args.Name == marketName).FirstOrDefault();
			if (market == null) {
				market = new Market(marketName);
				set.Add(market);
				await session.SaveChangesAsync();
			}

			DateTime startDate = new DateTime(1980, 1, 1);
			market.ContractSpec.SetDateLevel<ContractSpec, int>(new ContractSpec(market.Id, startDate, 1));
			market.ContractSpec.SetDateLevel<ContractSpec, int>(new ContractSpec(market.Id, new DateTime(1980, 2, 1), 2));
			market.ContractSpec.SetDateLevel<ContractSpec, int>(new ContractSpec(market.Id, new DateTime(1980, 3, 1), 3));
			market.ContractSpec.SetDateLevel<ContractSpec, int>(new ContractSpec(market.Id, new DateTime(1980, 4, 1), 3));
			await session.SaveChangesAsync();
			market.ContractSpec.SetDateLevel<ContractSpec, int>(new ContractSpec(market.Id, new DateTime(1980, 3, 1), 2));
			await session.SaveChangesAsync();
			market.ContractSpec.Clear();
			await session.SaveChangesAsync();
		}

		//[Fact(Skip = "require database")]
		[Fact]
		public async Task TestSpreadSpecPersistance() {
			string marketName = "test";
			using var scope = host.Create();
			var session = scope.Get<SampleDbSession>();
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
	}
}
