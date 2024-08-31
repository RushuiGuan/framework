using Microsoft.Extensions.Logging;

namespace Albatross.CommandLine {
	public record class GlobalOptions {
		public bool Clipboard { get; set; }
		public LogLevel? Log {get; set; }	
		public bool Benchmark { get; set; }
	}
}
