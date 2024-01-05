using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Logging {
	public static class Extensions {
		public static void RemoveLegacySlackSinkOptions() {
			Environment.SetEnvironmentVariable("Serilog__WriteTo__SlackSink__Args__SlackSinkOptions__WebHookUrl", null);
		}

		public static IServiceCollection AddCustomLogger(this IServiceCollection services) {
			services.AddSingleton<IGetLoggerName, GetLoggerName>();
			services.AddSingleton(typeof(ILogger<>), typeof(CustomLogger<>));
			return services;
		}
	}
}
