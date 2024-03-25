using Albatross.Hosting.Utility;
using CommandLine;
using System.Threading.Tasks;

namespace Sample.Messaging.CodeGen {
	class Program {
		static Task<int> Main(string[] args) {
			return Parser.Default.Run(args, typeof(Program).Assembly);
		}
	}
}
