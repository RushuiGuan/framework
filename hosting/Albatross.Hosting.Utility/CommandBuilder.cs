using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Hosting.Utility {
	public class CommandBuilder {
		Dictionary<Type, Type> dict = new Dictionary<Type, Type>();
		public CommandBuilder AddUtility<TOption, TUtility>() where TUtility : IUtility<TOption>{
			dict.Add(typeof(TOption), typeof(TUtility));
			return this;
		}
		public Task<int> Run(Parser parser, string[] args) {
			ParserResult<Object> parserResult = parser.ParseArguments(args, dict.Keys.ToArray());
			return parserResult.MapResult<object, Task<int>>(async opt => await Extensions.RunAsync(opt, dict), err => Task.FromResult(1));
		}
	}
}
