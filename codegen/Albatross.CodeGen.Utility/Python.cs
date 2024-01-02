using Albatross.CodeGen.Python.Models;
using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Utility {
	[Verb("python")]
	public class PythonOptions :BaseOption{
	}
	public class Python : UtilityBase<PythonOptions> {
		public Python(PythonOptions option) : base(option) {
		}
		public Task<int> RunUtility(ILogger logger) {
			new Module("test") {
				Classes = {
					new Class("test") {
						Methods = { 
							new Method("m1"),
							new Method("m2"),
						}
					},
					new Class("test2") {
						Methods = {
							new Method("m3"),
							new Method("m4"),
						}
					},
				}
			}.Generate(System.Console.Out);
			return Task.FromResult(0);
		}
	}
}
