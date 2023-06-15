using Albatross.Config;
using Albatross.Hosting.Utility;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleProject.Proxy;
using Serilog;

namespace SampleProject.Utility {
	public class MyUtilityBase<T> : UtilityBase<T> where T : BaseOption {
		protected MyUtilityBase(T option) : base(option) {
		}
		protected override void ConfigureLogging(LoggerConfiguration cfg) {
			cfg.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
				.MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error)
				.WriteTo
				.Console(outputTemplate: SetupSerilog.DefaultOutputTemplate)
				.Enrich.FromLogContext();
		}
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environment, IServiceCollection services) {
			base.RegisterServices(configuration, environment, services);
			services.AddSampleProjectProxy();
		}
	}
}
