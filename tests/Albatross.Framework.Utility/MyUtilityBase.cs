using Albatross.Config;
using Albatross.Hosting.Utility;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Albatross.Framework.Utility {
	public abstract class MyUtilityBase<T> : UtilityBase<T> {
		protected MyUtilityBase(T option) : base(option) {
		}

		protected override void ConfigureLogging(LoggerConfiguration cfg) {
			SetupSerilog.UseConfigFile(cfg, "serilog.json", null);
		}

		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environmentSetting, IServiceCollection services) {
			base.RegisterServices(configuration, environmentSetting, services);
			services.AddConfig<ProgramSetting>();
		}
	}
}
