using Albatross.Hosting.Utility;
using CommandLine;
using System;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.Utility {
	class Program {
		static Task<int> Main(string[] args) {
			var _ = typeof(Microsoft.CodeAnalysis.CSharp.Conversion);
			return Parser.Default.Run(args, typeof(Program).Assembly);
		}
	}
}
