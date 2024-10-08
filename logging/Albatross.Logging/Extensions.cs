using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Logging {
	public static class Extensions {
		public static void RemoveLegacySlackSinkOptions() {
			Environment.SetEnvironmentVariable("Serilog__WriteTo__SlackSink__Args__SlackSinkOptions__WebHookUrl", null);
		}

		/// <summary>
		/// Add a custom logger that shortens the logger name by removing the namespace prefix
		/// If `exclusive` is false, only the logger name that starts with the namespace prefix will 
		/// be shortened.  Otherwise only the logger name that does not start with the
		/// namespace prefix will be shortened.
		/// </summary>
		public static IServiceCollection AddShortenLoggerName(this IServiceCollection services, bool exclusive, params string[] namespacePrefix) {
			services.AddSingleton<IGetLoggerName>(new GetShortenedLoggerNameByNamespacePrefix(exclusive, namespacePrefix));
			services.AddSingleton(typeof(ILogger<>), typeof(CustomLogger<>));
			return services;
		}
	}
}
