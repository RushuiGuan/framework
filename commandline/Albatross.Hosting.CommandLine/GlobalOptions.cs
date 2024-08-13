using System.CommandLine;
using System.IO;

namespace Albatross.Hosting.CommandLine {
	public class GlobalOptions {
		public FileInfo? Output { get; set; }
		public bool Clipboard { get; set; }
		public bool Verbose { get; set; }
		public bool Information { get; set; }
		public bool Warning { get; set; }
		public bool Debug { get; set; }
		public bool Benchmark { get; set; }
	}
}
