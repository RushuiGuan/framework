using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;

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

		public static DateTime GetEventSourceFileTimeStamp(this string fileName) {
			fileName = Path.GetFileNameWithoutExtension(fileName);
			var index = fileName.LastIndexOf('_');
			if (index == -1) {
				throw new ArgumentException($"Invalid file name: {fileName}");
			} else {
				var text = fileName.Substring(index + 1);
				if (DateTime.TryParseExact(text, LogFileTimeStampFormat, null, System.Globalization.DateTimeStyles.None, out var timeStamp)) {
					return DateTime.SpecifyKind(timeStamp, DateTimeKind.Utc);
				} else {
					throw new ArgumentException($"Invalid file name: {fileName}");
				}
			}
		}
	}
}