using CommandLine;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Albatross.Hosting.Utility {
	[Verb("show-env")]
	public class ShowEnvironmentOption {}

	public class ShowEnvironment: UtilityBase<ShowEnvironmentOption> {

		public ShowEnvironment(ShowEnvironmentOption option):base(option) {
		}

		public Task<int> RunUtility(IConfiguration configuration) {

			string environment = System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
			logger.LogInformation("DOTNET_ENVIRONMENT Variable: {environment}", environment);

			var section = configuration.GetSection("connectionStrings");
			foreach(var item in section.GetChildren()) {
				logger.LogInformation("Connection String: {name} {value}", item.Key, item.Value);
			}

			section = configuration.GetSection("endpoints");
			foreach (var item in section.GetChildren()) {
				logger.LogInformation("Endpoint: {name} {value}", item.Key, item.Value);
			}

			return Task.FromResult(0);
		}
	}
}
