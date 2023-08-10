using Albatross.Config;
using Albatross.Hosting.Utility;
using Albatross.Logging;
using Albatross.Repository.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Albatross.Repository.Test {
	public abstract class MyUtilityBase<T> : UtilityBase<T> {
		protected MyUtilityBase(T option) : base(option) {
		}

		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environmentSetting, IServiceCollection services) {
			base.RegisterServices(configuration, environmentSetting, services);
			services.AddSqlServerWithContextPool<MyDbSession>(provider => Constant.ConnectionString);
			services.AddScoped<SqlServerMigration<MySqlServerMigration>>();
			services.AddScoped<MySqlServerMigration>(provider => {
				return new MySqlServerMigration(Constant.ConnectionString);
			});
		}
	}
}
