using Albatross.EFCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test.MyNamespace {
	public class FutureMarketEntityMap : EntityMap<FutureMarket> {
		public override void Map(EntityTypeBuilder<FutureMarket> builder) {
			base.Map(builder);
		}
	}

	public class TestMyDbSession : IClassFixture<MyTestHost> {
		private readonly MyTestHost host;

		public TestMyDbSession(MyTestHost host) {
			this.host = host;
		}

		[Fact]
		public void TestGetEntityModels() {
			var items = this.GetType().Assembly.GetEntityModels(null);
			Assert.NotEmpty(items);

			items = this.GetType().Assembly.GetEntityModels("Albatross.EFCore.Test.MyNamespace");
			Assert.Single(items);

			items = this.GetType().Assembly.GetEntityModels("Albatross.EFCore.Test.MyNamespace.");
			Assert.Single(items);
		}

		[Fact(Skip = "require database")]
		public async Task TestTickSizePersistance() {
			string marketName = "test";
			DateTime startDate = new DateTime(1980, 1, 1);
			using var scope = host.Create();
			var session = scope.Get<MyDbSession>();
			var set = session.DbContext.Set<FutureMarket>();
			var market = set
				.Include(args => args.TickSizes)
				.Where(args => args.Name == marketName).FirstOrDefault();
			if (market == null) {
				market = new FutureMarket(marketName);
				set.Add(market);
				await session.SaveChangesAsync();
			}
			market.TickSizes.SetDateLevel<TickSize, int>(new TickSize(market.Id, startDate, 1));
			market.TickSizes.SetDateLevel<TickSize, int>(new TickSize(market.Id, new DateTime(1980, 2, 1), 2));
			market.TickSizes.SetDateLevel<TickSize, int>(new TickSize(market.Id, new DateTime(1980, 3, 1), 3));
			market.TickSizes.SetDateLevel<TickSize, int>(new TickSize(market.Id, new DateTime(1980, 4, 1), 3));
			await session.SaveChangesAsync();
			market.TickSizes.SetDateLevel<TickSize, int>(new TickSize(market.Id, new DateTime(1980, 3, 1), 2));
			await session.SaveChangesAsync();
		}
	}
}
