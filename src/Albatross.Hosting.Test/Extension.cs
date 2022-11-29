using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Albatross.Hosting.Test {
	public static class Extension {
		public static T CreateAsyncMockSession<T, K>(this IEnumerable<K> items) where T : class, IDbSession where K:class{
			return CreateAsyncMockSession<T>(mock => mock.CreateAsyncDbSet<K>(items));
		}
		public static T CreateAsyncMockSession<T>(Action<Mock<DbContext>> setupContext)where T : class , IDbSession {
			var dbContextMock = new Mock<DbContext>();
			setupContext(dbContextMock);
			var sessionMock = new Mock<T>();
			sessionMock.Setup(args => args.DbContext).Returns(dbContextMock.Object);
			return sessionMock.Object;
		}
		public static void CreateAsyncDbSet<T>(this Mock<DbContext> dbContextMock, IEnumerable<T> items) where T : class {
			dbContextMock.Setup(args => args.Set<T>()).Returns(items.CreateAsyncDbSet<T>().Object);
		}
		public static Mock<DbSet<T>> CreateAsyncDbSet<T>(this IEnumerable<T> items) where T:class {
			var data = new AsyncTestData<T>(items);
			var mock = new Mock<DbSet<T>>();
			mock.As<IQueryable<T>>().Setup(args => args.Expression).Returns(data.Expression);
			mock.As<IQueryable<T>>().Setup(args => args.Provider).Returns(data.Provider);
			mock.As<IAsyncEnumerable<T>>().Setup(args=>args.GetAsyncEnumerator(CancellationToken.None))
				.Returns(()=> data.GetAsyncEnumerator());
			return mock;
		}
	}
}