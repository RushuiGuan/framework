using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Logging {
	public static class Extensions {
		public static void RemoveLegacySlackSinkOptions() {
			Environment.SetEnvironmentVariable("Serilog__WriteTo__SlackSink__Args__SlackSinkOptions__WebHookUrl", null);
		}

		public static IServiceCollection AddShortenLoggerName(this IServiceCollection services, bool exclusive, params string[] namespacePrefix) {
			services.AddSingleton<IGetLoggerName>(new GetShortenedLoggerNameByNamespacePrefix(exclusive, namespacePrefix));
			services.AddSingleton(typeof(ILogger<>), typeof(CustomLogger<>));
			return services;
		}
	}
}
