using CommandLine;

namespace Albatross.Hosting.Utility {
	public class BaseOption {
		[Option('o', "out", Required = false, HelpText = "Output file name")]
		public string Output { get; set; }

		[Option('v', "verbose")]
		public bool Verbose { get; set; }

		[Option('c', "clipboard")]
		public bool Clipboard { get; set; }
	}
}
