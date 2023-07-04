using Albatross.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SampleProject.Daemon {
	public class MySetup : Setup {
		public MySetup(string[] args) : base(args) {
		}

		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddSampleProjectCommandBus();
		}
	}
}