using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Albatross.Framework.Utility {
	[Verb("input-test")]
	public class InputTestOption{
		[Option('i', "input", Required =true)]
		public IEnumerable<int> Array { get; set; }

		[Option('s', "string", Required = false)]
		public IEnumerable<string> TextArray { get; set; }
	}

	public class InputTest: MyUtilityBase<InputTestOption> {

		public InputTest(InputTestOption option):base(option) {
		}


		protected override Task<int> RunUtility() {
			if (Options.Array != null) {
				foreach (var i in Options.Array) {
					Console.WriteLine(i);
				}
			}
			if(Options.TextArray != null) {
				foreach (var i in Options.TextArray) {
					Console.WriteLine(i);
				}
			}
			return Task.FromResult(1);
		}
	}
}
