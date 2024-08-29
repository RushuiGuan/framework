using Albatross.Config;
using Albatross.Hosting.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Proxy;

namespace Sample.Utility {

	public class MyUtilityBase<T> : UtilityBase<T> where T : BaseOption {
		protected MyUtilityBase(T option) : base(option) {
		}
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environment, IServiceCollection services) {
			base.RegisterServices(configuration, environment, services);
			services.AddSampleProjectProxy();
		}
	}
}
