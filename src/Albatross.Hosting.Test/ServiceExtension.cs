using Albatross.Authentication.Core;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;

namespace Albatross.Hosting.Test {
	public static class ServiceExtension {
		public static IServiceCollection AddTestPrincipalProvider(this IServiceCollection svc, string provider, string account) {
			return svc.AddSingleton<IGetCurrentUser>(new GetCurrentTestUser(provider, account));
		}

		public static void GetDbSession<T, K>(K value) where T: class {
			var providerMock = new Mock<IAsyncQueryProvider>();
			providerMock.Setup(args => args.ExecuteAsync<K>(It.IsAny<Expression>(), CancellationToken.None))
				.Returns(value);

			var dbsetMock = new Mock<DbSet<T>>();
			dbsetMock.As<IQueryable<T>>()
				.Setup(m => m.Provider)
				.Returns(providerMock.Object);

			var dbSessionMock = new Mock<IDbSession>();

			return dbsetMock.Object;
		}
	}
}