using Albatross.Config;
using Albatross.Hosting.Utility;
using Albatross.Logging;
using Albatross.Messaging.Commands;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleProject.Commands;
using SampleProject.Proxy;
using Serilog;
using Serilog.Filters;
using System;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	public class MyUtilityBase<T> : UtilityBase<T> where T : BaseOption {
		protected MyUtilityBase(T option) : base(option) {
		}
		protected override void ConfigureLogging(LoggerConfiguration cfg) {
			cfg.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
				.MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error)
				.WriteTo.Logger(log_cfg => log_cfg.Filter.ByExcluding(Matching.FromSource("message-entry")).WriteTo.Console(outputTemplate: SetupSerilog.DefaultOutputTemplate))
				.WriteTo.Logger(log_cfg => log_cfg.Filter.ByIncludingOnly(Matching.FromSource("message-entry")).WriteTo.Console(outputTemplate: "msg-entry {Message:lj}"))
				.Enrich.FromLogContext();
		}
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environment, IServiceCollection services) {
			base.RegisterServices(configuration, environment, services);
			services.AddSampleProjectProxy();
			services.AddSampleProjectCommands()
				.AddCommandClient()
				.AddDefaultDealerClientConfig();
		}
		public override Task Init(IConfiguration configuration, IServiceProvider provider) {
			base.Init(configuration, provider);
			provider.UseDealerClient();
			return Task.CompletedTask;
		}
	}
}
