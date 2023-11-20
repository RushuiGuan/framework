using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Config.UnitTest {
	public class MyTestHost : Hosting.Test.TestHost {
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddConfig<ProgramSetting>();
			services.AddConfig<SingleValueConfig>();
			services.AddConfig<MySetting>();
			services.AddConfig<ConfigWithNoKey>();
			services.AddConfig<DbConfig>();
			services.AddConfig<ValidationTest>();
		}
	}
}