using Albatross.Config;
using Albatross.Hosting.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Framework.Utility {
	public abstract class MyUtilityBase<T> : UtilityBase<T> {
		protected MyUtilityBase(T option) : base(option) {
		}

		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environmentSetting, IServiceCollection services) {
			base.RegisterServices(configuration, environmentSetting, services);
			services.AddTestProxy();
		}
	}
}
