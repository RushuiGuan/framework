﻿using Albatross.Hosting.Utility;
using CommandLine;
using System.Threading.Tasks;

namespace Albatross.Repository.Test {
	class Program {
		static Task<int> Main(string[] args) {
			return Parser.Default.Run(args, typeof(Program).Assembly);
		}
	}
}
