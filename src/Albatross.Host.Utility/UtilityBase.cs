using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace Albatross.Host.Utility {
	public abstract class UtilityBase<Option> : IUtility<Option> {
		public TextWriter Out => System.Console.Out;
		public TextWriter Error => System.Console.Error;

		public Option Options { get; }
		protected IServiceProvider Provider { get; private set; }
		protected IConfiguration Configuration { get; private set; }
		protected IHost host;

		public UtilityBase(Option option) {
			this.Options = option;

			host = Microsoft.Extensions.Hosting.Host
						 .CreateDefaultBuilder()
						 .UseSerilog()
						 .ConfigureServices((ctx, svc) => RegisterServices(ctx.Configuration, svc))
						 .Build();

			Init(host.Services.GetRequiredService<IConfiguration>(), host.Services);
		}

		public virtual void RegisterServices(IConfiguration configuration, IServiceCollection services) { }
		public abstract int Run();
		public virtual void Init(IConfiguration configuration, IServiceProvider provider) { }

		public void Dispose() {
			this.host.Dispose();
		}
	}
}
