using Albatross.Hosting.Utility;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Albatross.Config;
using System;

namespace Albatross.Caching.Utility {
	public abstract class MyUtilityBase<T> : UtilityBase<T> {
		protected MyUtilityBase(T option) : base(option) {
		}
		protected override void ConfigureLogging(LoggerConfiguration cfg) {
			cfg.MinimumLevel.Information()
				.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
				.MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error)
				.WriteTo
				.Console(outputTemplate: SetupSerilog.DefaultOutputTemplate)
				.Enrich.FromLogContext();
		}
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environment, IServiceCollection services) {
			base.RegisterServices(configuration, environment, services);
			services.AddConfig<RedisConfig>();
			services.AddSingleton<IRedisConnection, RedisConnection>();
		}
	}
}
