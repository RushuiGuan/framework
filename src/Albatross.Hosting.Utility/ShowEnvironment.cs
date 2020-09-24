using CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Albatross.Config.Core;
using Microsoft.Extensions.Hosting;

namespace Albatross.Hosting.Utility {
	[Verb("show-env")]
	public class ShowEnvironmentOption {}

	public class ShowEnvironment: UtilityBase<ShowEnvironmentOption> {

		public ShowEnvironment(ShowEnvironmentOption option):base(option) {
		}

		public override Task<int> RunAsync() {

			string environment = System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
			logger.LogInformation("DOTNET_ENVIRONMENT Variable: {environment}", environment);

			var setting = this.host.Services.GetRequiredService<ProgramSetting>();
			logger.LogInformation("Program Setting: {environment}", setting?.Environment);

			var hostEnv = this.host.Services.GetRequiredService<IHostEnvironment>();
			logger.LogInformation("Host Environment: {hostEnvironment}", hostEnv.EnvironmentName);
			return Task.FromResult(0);
		}
	}
}
