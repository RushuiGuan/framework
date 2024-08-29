using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Hosting;

namespace Sample.CommandLine {
	public static class CommandRegistrationExtensions {
		public static IServiceCollection RegisterCommands(this IServiceCollection services) {
			services.AddOptions<MyCommandOptions>().BindCommandLine();
			return services;
		}
	}
}