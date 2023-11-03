using Serilog.Configuration;
using Serilog;
using System;

namespace Albatross.Logging {
	public static class SerilogEnricherExtensions {
		public static LoggerConfiguration WithErrorMessage(this LoggerEnrichmentConfiguration enrichmentConfiguration) {
			if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
			return enrichmentConfiguration.With<ErrorMessageEnricher>();
		}
	}
}
