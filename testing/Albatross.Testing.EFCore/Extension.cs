using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Albatross.Testing.EFCore {
	public static class Extension {
		/// <summary>
		/// This method can create a mock db session object using the provided enumerable items.  the session can be query 
		/// using both async in sync mode.
		/// </summary>
		public static T CreateAsyncMockSession<T, K>(this IEnumerable<K> items) where T : class, IDbSession where K : class {
			return CreateAsyncMockSession<T>(mock => mock.CreateAsyncDbSet<K>(items));
		}
		public static T CreateAsyncMockSession<T>(Action<Mock<DbContext>> setupContext) where T : class, IDbSession {
			var dbContextMock = new Mock<DbContext>();
			setupContext(dbContextMock);
			var sessionMock = new Mock<T>();
			sessionMock.Setup(args => args.DbContext).Returns(dbContextMock.Object);
			return sessionMock.Object;
		}
		public static void CreateAsyncDbSet<T>(this Mock<DbContext> dbContextMock, IEnumerable<T> items) where T : class {
			dbContextMock.Setup(args => args.Set<T>()).Returns(items.CreateAsyncDbSet<T>().Object);
		}
		public static Mock<DbSet<T>> CreateAsyncDbSet<T>(this IEnumerable<T> items) where T : class {
			var data = new TestAsyncEnumerableQuery<T>(items);
			var mock = new Mock<DbSet<T>>();
			mock.As<IEnumerable<T>>().Setup(args => args.GetEnumerator()).Returns(((IEnumerable<T>)data).GetEnumerator());
			mock.As<IQueryable<T>>().Setup(args => args.Expression).Returns(((IQueryable)data).Expression);
			mock.As<IQueryable<T>>().Setup(args => args.Provider).Returns(((IQueryable)data).Provider);
			mock.As<IAsyncEnumerable<T>>().Setup(args => args.GetAsyncEnumerator(CancellationToken.None))
				.Returns(() => data.GetAsyncEnumerator());
			return mock;
		}
	}
}