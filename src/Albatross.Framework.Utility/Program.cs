using Albatross.Logging;
using Albatross.Host.Utility;
using CommandLine;
using System.Threading.Tasks;

namespace Albatross.Framework.Utility {
	class Program {
		static Task<int> Main(string[] args) {
			return Parser.Default.Run(args, typeof(ReferenceTreeSearch), typeof(LoggingTest));
		}
	}
}
