using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Albatross.Messaging.EventSource {
	public static class Extensions {
		public static IServiceCollection AddNoOpDataLogging(this IServiceCollection services) {
			services.TryAddSingleton<IEventReader, NoOpEventReader>();
			services.TryAddSingleton<IEventWriter, NoOpEventWriter>();
			return services;
		}

		public const string LogFileExtension = ".log";
		public const string LogFileTimeStampFormat = "yyyyMMddTHHmmssfff";
		public static string GetEventSourceFileName(this string filename, DateTime? timeStamp = null)
			=> $"{filename}_{(timeStamp ?? DateTime.UtcNow).ToString(LogFileTimeStampFormat)}{LogFileExtension}";
		public static string GetEventSourceFilePattern(this string fileName) => $"{fileName}_*{LogFileExtension}";
	}
}