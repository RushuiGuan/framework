using Albatross.Logging;
using Albatross.Host.Utility;
using CommandLine;

namespace Albatross.Framework.Utility {
	class Program {
		static int Main(string[] args) {
			using var setup = new SetupSerilog();
			return Parser.Default.Run(args, typeof(ReferenceTreeSearch), typeof(LoggingTest));
		}
	}
}
