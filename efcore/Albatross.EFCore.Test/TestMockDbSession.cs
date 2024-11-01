using Albatross.Testing.EFCore;
using Microsoft.EntityFrameworkCore;
using Sample.EFCore.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestMockDbSession {
		readonly static Market[] Markets = new Market[] {
			new Market("L"),
			new Market("C"),
			new Market("NG"),
		};

		/// <summary>
		/// This is a special execute that does not leverage IAsyncQueryProvider.  It uses IAsyncEnumerable<T> instead because there is not expression involved
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task AsyncExecuteToArray() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var result = await session.DbContext
				.Set<Market>()
				.ToArrayAsync();
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(Markets.Count(), result.Count());
		}

		[Fact]
		public void ExecuteToArray() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var set = session.DbContext.Set<Market>();
			var result = set.ToArray();
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(Markets.Count(), result.Count());
		}

		[Fact]
		public void CreateWhereQuery() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var query = session.DbContext
				.Set<Market>()
				.Where(args => args.Id > 0);
			Assert.NotNull(query);
		}

		[Fact]
		public async Task AsyncExecuteWhereQuery() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var result = await session.DbContext
				.Set<Market>()
				.Where(args => args.Name.Contains("C"))
				.ToArrayAsync();
			Assert.NotEmpty(result);
		}

		[Fact]
		public void ExecuteWhereQuery() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var result = session.DbContext
				.Set<Market>()
				.Where(args => args.Name.Contains("C"))
				.ToArray();
			Assert.NotEmpty(result);
			Assert.Collection(result, args => Assert.Equal("C", args.Name));
		}

		[Fact]
		public void CreateIncludeQuery() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var result = session.DbContext
				.Set<Market>()
				.Include(args => args.ContractSpec)
				.Where(args => args.Name.Contains("C"));
			Assert.NotNull(result);
		}

		[Fact]
		public async Task AsyncExecuteIncludeQuery() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var result = await session.DbContext
				.Set<Market>()
				.Include(args => args.ContractSpec)
				.Where(args => args.Name.Contains("C"))
				.ToArrayAsync();
			Assert.NotEmpty(result);
		}

		[Fact]
		public void ExecuteIncludeQuery() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var result = session.DbContext
				.Set<Market>()
				.Include(args => args.ContractSpec)
				.Where(args => args.Name.Contains("C"))
				.ToArray();
			Assert.NotEmpty(result);
		}

		[Fact]
		public void ExecuteFirstWithoutPedicate() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var result = session.DbContext
				.Set<Market>()
				.Include(args => args.ContractSpec)
				.First();
			Assert.NotNull(result);
			Assert.Same(Markets.First(), result);
		}

		[Fact]
		public async Task AsyncExecuteFirstWithoutPedicate() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var result = await session.DbContext
				.Set<Market>()
				.Include(args => args.ContractSpec)
				.FirstAsync();
			Assert.NotNull(result);
			Assert.Same(Markets.First(), result);
		}
		[Fact]
		public void ExecuteFirstWithPedicate() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var result = session.DbContext
				.Set<Market>()
				.Include(args => args.ContractSpec)
				.First(args => args.Name == "C");
			Assert.NotNull(result);
			Assert.Equal("C", result.Name);
		}

		[Fact]
		public async Task AsyncExecuteFirstWithPedicate() {
			var session = Markets.CreateAsyncMockSession<ISampleDbSession, Market>();
			var result = await session.DbContext
				.Set<Market>()
				.Include(args => args.ContractSpec)
				.FirstAsync(args => args.Name == "C");
			Assert.NotNull(result);
			Assert.Equal("C", result.Name);
		}
	}
}