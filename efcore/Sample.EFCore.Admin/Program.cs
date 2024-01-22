using Albatross.Hosting.Utility;
using CommandLine;
using System.Threading.Tasks;

namespace Sample.EFCore.Admin {
	internal class Program {
		static Task<int> Main(string[] args) {
			return Parser.Default.Run(args, typeof(Program).Assembly);
		}
	}
}
