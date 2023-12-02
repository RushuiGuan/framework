using Albatross.Hosting.Utility;
using CommandLine;

namespace Sample.Logging.Utility {
	internal class Program {
		static Task<int> Main(string[] args) {
			return Parser.Default.Run(args, typeof(Program).Assembly);
		}
	}
}
