﻿using Albatross.Hosting.Utility;
using CommandLine;
using System.Threading.Tasks;

namespace Sample.Utility {
	public class Program {
		public static Task Main(string[] args) {
			return Parser.Default.Run(args, typeof(Program).Assembly);
		}
	}
}