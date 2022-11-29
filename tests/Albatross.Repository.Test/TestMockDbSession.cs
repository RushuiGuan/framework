using Albatross.Hosting.Test;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Repository.Test {
	public class TestMockDbSession {
		readonly static FutureMarket[] Markets = new FutureMarket[] { 
			new FutureMarket("C"),
			new FutureMarket("NG"),
			new FutureMarket("L"),
		};

		[Fact]
		public async Task AsyncToArray() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = await session.DbContext
				.Set<FutureMarket>()
				.ToArrayAsync();
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(Markets.Count(), result.Count());
		}

		[Fact]
		public async Task AsyncWhereQuery() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = await session.DbContext
				.Set<FutureMarket>()
				.Where(args=>args.Id > 0)
				.ToArrayAsync();
			Assert.NotEmpty(result);
		}

		[Fact]
		public async Task AsyncIncludeQuery() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = await session.DbContext
				.Set<FutureMarket>()
				.Include(args=>args.TickSizes)
				.Where(args => args.Id > 0)
				.ToArrayAsync();
			Assert.NotEmpty(result);
		}
		[Fact]
		public async Task AsyncFirstWithoutPedicate() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = await session.DbContext
				.Set<FutureMarket>()
				.Include(args => args.TickSizes)
				.FirstAsync();
			Assert.NotNull(result);
		}
		[Fact]
		public async Task AsyncFirstWithPedicate() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = await session.DbContext
				.Set<FutureMarket>()
				.Include(args => args.TickSizes)
				.FirstAsync(args => true);
			Assert.NotNull(result);
			Assert.Equal("C", result.Name);
		}
	}
}
