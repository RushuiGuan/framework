using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Albatross.Messaging.DataLogging {
	public static class Extensions {
		public static IServiceCollection AddNoOpDataLogging(this IServiceCollection services) {
			services.TryAddSingleton<ILogReader, NoOpDataLogReader>();
			services.TryAddSingleton<ILogWriter, NoOpDataLogWriter>();
			return services;
		}

		public const string LogFileExtension = ".log";
		public const string LogFileTimeStampFormat = "yyyyMMddHHmmssfff";
		public static string GetLogFileName(this string filename, DateTime? timeStamp = null)
			=> $"{filename}_{(timeStamp ?? DateTime.UtcNow).ToString(LogFileTimeStampFormat)}{LogFileExtension}";
		public static string GetLogFilePattern(this string fileName) => $"{fileName}_*{LogFileExtension}";
	}
}
