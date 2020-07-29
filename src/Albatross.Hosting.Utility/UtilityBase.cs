using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Hosting.Utility {
	/// <summary>
	/// The utility base acts as the bootstrapper for dependency injections.
	/// </summary>
	/// <typeparam name="Option"></typeparam>
	public abstract class UtilityBase<Option> : IUtility<Option> {
		public TextWriter Out => System.Console.Out;
		public TextWriter Error => System.Console.Error;

		public Option Options { get; }
		public Microsoft.Extensions.Logging.ILogger logger;
		protected IServiceProvider Provider => host.Services;
		protected IHost host;
		private Serilog.Core.Logger serilogLogger;

		protected virtual Serilog.Core.Logger SetupLogging(Option option) {
			return new SetupSerilog().UseConsole(LogEventLevel.Debug).Create();
		}


		public UtilityBase(Option option) {
			this.Options = option;
			serilogLogger = SetupLogging(option);
			host = Microsoft.Extensions.Hosting.Host
						 .CreateDefaultBuilder()
						 .UseSerilog()
						 .ConfigureServices((ctx, svc) => RegisterServices(ctx.Configuration, svc))
						 .Build();

			logger = host.Services.GetRequiredService(typeof(Microsoft.Extensions.Logging.ILogger<>).MakeGenericType(this.GetType())) as Microsoft.Extensions.Logging.ILogger;
			logger.LogInformation("Logging initialized for {type} instance", this.GetType().Name);
			Init(host.Services.GetRequiredService<IConfiguration>(), host.Services);
		}

		public virtual void RegisterServices(IConfiguration configuration, IServiceCollection services) { }
		public abstract Task<int> RunAsync();
		public virtual void Init(IConfiguration configuration, IServiceProvider provider) { }

		public void Dispose() {
			logger.LogDebug("Disposing UtilityBase");
			this.host.Dispose();
			logger.LogDebug("CloseAndFlush Logging");
			serilogLogger.Dispose();
		}
	}
}
