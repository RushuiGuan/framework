using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Host.Utility {
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

		protected void SetupLogging(Option option) {
			new SetupSerilog().UseConsoleAndFile(LogEventLevel.Information, "out.log");
		}


		public UtilityBase(Option option) {
			this.Options = option;
			SetupLogging(option);
			host = Microsoft.Extensions.Hosting.Host
						 .CreateDefaultBuilder()
						 .UseSerilog()
						 .ConfigureServices((ctx, svc) => RegisterServices(ctx.Configuration, svc))
						 .Build();


			logger = host.Services.GetRequiredService(typeof(Microsoft.Extensions.Logging.ILogger<>).MakeGenericType(this.GetType())) as Microsoft.Extensions.Logging.ILogger;

			Init(host.Services.GetRequiredService<IConfiguration>(), host.Services);

		}

		public virtual void RegisterServices(IConfiguration configuration, IServiceCollection services) { }
		public abstract Task<int> RunAsync();
		public virtual void Init(IConfiguration configuration, IServiceProvider provider) { }

		public void Dispose() {
			this.host.Dispose();
			Log.CloseAndFlush();
		}
	}
}
