using Albatross.Hosting.Utility;
using CommandLine;
using System;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Utility {
	class Program {
		static Task<int> Main(string[] args) {
			return Parser.Default.Run(args, typeof(Program).Assembly);
		}
	}
}
