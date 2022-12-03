using Albatross.Hosting.Test;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Repository.Test {
	public class TestMockDbSession {
		readonly static FutureMarket[] Markets = new FutureMarket[] { 
			new FutureMarket("L"),
			new FutureMarket("C"),
			new FutureMarket("NG"),
		};

		/// <summary>
		/// This is a special execute that does not leverage IAsyncQueryProvider.  It uses IAsyncEnumerable<T> instead because there is not expression involved
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task AsyncExecuteToArray() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = await session.DbContext
				.Set<FutureMarket>()
				.ToArrayAsync();
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(Markets.Count(), result.Count());
		}

		[Fact]
		public void ExecuteToArray() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var set = session.DbContext.Set<FutureMarket>();
			var result = set.ToArray();
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(Markets.Count(), result.Count());
		}

		[Fact]
		public void CreateWhereQuery() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var query = session.DbContext
				.Set<FutureMarket>()
				.Where(args => args.Id > 0);
			Assert.NotNull(query);
		}

		[Fact]
		public async Task AsyncExecuteWhereQuery() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = await session.DbContext
				.Set<FutureMarket>()
				.Where(args=>args.Name.Contains("C"))
				.ToArrayAsync();
			Assert.NotEmpty(result);
		}

		[Fact]
		public void ExecuteWhereQuery() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = session.DbContext
				.Set<FutureMarket>()
				.Where(args => args.Name.Contains("C"))
				.ToArray();
			Assert.NotEmpty(result);
			Assert.Collection(result, args => Assert.Equal("C", args.Name));
		}

		[Fact]
		public void CreateIncludeQuery() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = session.DbContext
				.Set<FutureMarket>()
				.Include(args => args.TickSizes)
				.Where(args => args.Name.Contains("C"));
			Assert.NotNull(result);
		}

		[Fact]
		public async Task AsyncExecuteIncludeQuery() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = await session.DbContext
				.Set<FutureMarket>()
				.Include(args=>args.TickSizes)
				.Where(args => args.Name.Contains("C"))
				.ToArrayAsync();
			Assert.NotEmpty(result);
		}

		[Fact]
		public void ExecuteIncludeQuery() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = session.DbContext
				.Set<FutureMarket>()
				.Include(args => args.TickSizes)
				.Where(args => args.Name.Contains("C"))
				.ToArray();
			Assert.NotEmpty(result);
		}

		[Fact]
		public void ExecuteFirstWithoutPedicate() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = session.DbContext
				.Set<FutureMarket>()
				.Include(args => args.TickSizes)
				.First();
			Assert.NotNull(result);
			Assert.Same(Markets.First(), result);
		}

		[Fact]
		public async Task AsyncExecuteFirstWithoutPedicate() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = await session.DbContext
				.Set<FutureMarket>()
				.Include(args => args.TickSizes)
				.FirstAsync();
			Assert.NotNull(result);
			Assert.Same(Markets.First(), result);
		}
		[Fact]
		public void ExecuteFirstWithPedicate() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = session.DbContext
				.Set<FutureMarket>()
				.Include(args => args.TickSizes)
				.First(args => args.Name == "C");
			Assert.NotNull(result);
			Assert.Equal("C", result.Name);
		}

		[Fact]
		public async Task AsyncExecuteFirstWithPedicate() {
			var session = Markets.CreateAsyncMockSession<IMyDbSession, FutureMarket>();
			var result = await session.DbContext
				.Set<FutureMarket>()
				.Include(args => args.TickSizes)
				.FirstAsync(args => args.Name == "C");
			Assert.NotNull(result);
			Assert.Equal("C", result.Name);
		}
	}
}
