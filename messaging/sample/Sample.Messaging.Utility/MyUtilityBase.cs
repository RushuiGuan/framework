		
using Albatross.Config;
using Albatross.Hosting.Utility;
using Albatross.Logging;
using Albatross.Messaging.Commands;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Messaging.Commands;
using Sample.Messaging.Proxy;
using Serilog;
using Serilog.Core;
using Serilog.Filters;
using System;
using System.Threading.Tasks;

namespace Sample.Messaging.Utility {

	public class MyUtilityBase<T> : UtilityBase<T> where T : BaseOption {
		protected MyUtilityBase(T option) : base(option) {
		}
		protected override void ConfigureLogging(LoggerConfiguration cfg) {
			cfg.MinimumLevel.Information()
				.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
				.WriteTo.Logger(log_cfg => log_cfg.Filter.ByExcluding(Matching.WithProperty<string>(Constants.SourceContextPropertyName, source => source != null && source.EndsWith("message-entry")))
				.WriteTo.Console(outputTemplate: SetupSerilog.DefaultOutputTemplate))
				.WriteTo.Logger(log_cfg => log_cfg.Filter.ByIncludingOnly(Matching.WithProperty<string>(Constants.SourceContextPropertyName, source => source != null && source.EndsWith("message-entry")))
				.WriteTo.Console(outputTemplate: "{SourceContext}:{Message:lj}"))
				.Enrich.FromLogContext();
		}
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environment, IServiceCollection services) {
			base.RegisterServices(configuration, environment, services);
			services.AddSampleProjectProxy();
			services.AddCommandClient<MyCommandClient>()
				.AddDefaultDealerClientConfig();
		}
		public override async Task Init(IConfiguration configuration, IServiceProvider provider) {
			await base.Init(configuration, provider);
			provider.UseDealerClient();
			
		}
	}
}
