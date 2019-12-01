using Albatross.Logging;
using Albatross.Host.Utility;
using CommandLine;
using System.Threading.Tasks;

namespace Albatross.Framework.Utility {
	class Program {
		static Task<int> Main(string[] args) {
			using var setup = new SetupSerilog();
			return Parser.Default.Run(args, typeof(ReferenceTreeSearch), typeof(LoggingTest));
		}
	}
}
