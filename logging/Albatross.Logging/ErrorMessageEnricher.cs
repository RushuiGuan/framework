using Serilog.Core;
using Serilog.Events;

namespace Albatross.Logging {
	public class ErrorMessageEnricher : ILogEventEnricher {
		public const string ErrorMessagePropertyName = "ErrorMessage";

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
			var msg = logEvent.Exception?.Message ?? string.Empty;
			if (!string.IsNullOrEmpty(msg)) {
				msg = $" - {msg}";
			}
			var property = propertyFactory.CreateProperty(ErrorMessagePropertyName, msg);
			logEvent.AddPropertyIfAbsent(property);
		}
	}
}
