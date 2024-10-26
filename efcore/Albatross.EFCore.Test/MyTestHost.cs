using Albatross.Caching;
using Albatross.Caching.MemCache;
using Albatross.EFCore.Audit;
using Albatross.Hosting.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Sample.EFCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Albatross.EFCore.Test {
	public class MyTestHost : Hosting.Test.TestHost {
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddSample();
			services.AddMemCaching();
			services.AddCaching();
			services.AddBuiltInCache();
			services.AddAuditEventHandlers();
			services.AddTestPrincipalProvider("test", "test");
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