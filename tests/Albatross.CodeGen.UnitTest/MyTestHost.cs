using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.CodeGen.UnitTest {
	public class MyTestHost : TestHost{
		protected ServiceProvider provider;

		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddDefaultCodeGen();
		}
	}
}
