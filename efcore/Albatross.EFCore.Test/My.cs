using Albatross.Authentication;
using Albatross.Caching;
using Albatross.Caching.MemCache;
using Albatross.EFCore.Audit;
using Albatross.Testing.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Sample.EFCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Albatross.EFCore.Test {
	public static class My {
		public static void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			services.AddSample();
			services.AddMemCaching();
			services.AddCaching();
			services.AddBuiltInCache();
			services.AddAuditEventHandlers();
			services.AddSingleton(new GetCurrentTestUser("test"));
			services.AddSingleton<IGetCurrentUser>(provider => provider.GetRequiredService<GetCurrentTestUser>());
		}
		public static IHost Create() {
			return new TestHostBuilder().RegisterServices(My.RegisterServices).Build();
		}
		public static void GetDbSession<T, K>(K value) where T : class {
			var providerMock = new Mock<IAsyncQueryProvider>();
			providerMock.Setup(args => args.ExecuteAsync<K>(It.IsAny<Expression>(), CancellationToken.None))
				.Returns(value);

			var dbsetMock = new Mock<DbSet<T>>();
			dbsetMock.As<IQueryable<T>>()
				.Setup(m => m.Provider)
				.Returns(providerMock.Object);

			var dbSessionMock = new Mock<IDbSession>();
		}
	}
}