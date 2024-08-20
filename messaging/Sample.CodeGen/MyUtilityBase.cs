using Albatross.Config;
using Albatross.Hosting.Utility;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Sample.CodeGen {
	public abstract class MyUtilityBase<T> : UtilityBase<T> {
		protected MyUtilityBase(T option) : base(option) {
		}
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environmentSetting, IServiceCollection services) {
			base.RegisterServices(configuration, environmentSetting, services);
		}
	}
}
