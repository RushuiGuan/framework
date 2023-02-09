using Albatross.Hosting.Test;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Albatross.Repository.SqlServer;
using Albatross.Repository.ByEFCore;

namespace Albatross.Repository.Test {
	public class MyTestHost : TestHost {
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddSqlServerWithContextPool<MyDbSession>(provider => Constant.ConnectionString);
			services.AddScoped<SqlServerMigration<MySqlServerMigration>>();
			services.AddScoped<MySqlServerMigration>(provider => {
				return new MySqlServerMigration(Constant.ConnectionString);
			});
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