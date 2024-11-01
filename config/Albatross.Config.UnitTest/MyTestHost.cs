using Albatross.Testing.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Albatross.Config.UnitTest {
	public class My {
		public static IHost Create() {
			return new TestHostBuilder().WithAppSettingsConfiguration("testing").RegisterServices(RegisterServices).Build();
		}
		public static void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			services.AddConfig<ProgramSetting>();
			services.AddConfig<SingleValueConfig>();
			services.AddConfig<MySetting>();
			services.AddConfig<ConfigWithNoKey>();
			services.AddConfig<DbConfig>();
			services.AddConfig<ValidationTest>();
		}
	}
}