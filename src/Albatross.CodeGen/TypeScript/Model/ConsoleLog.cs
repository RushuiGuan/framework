using Albatross.CodeGen.Core;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Model {
	public class ConsoleLog : MethodCall  {
		public ConsoleLog(string text, params ICodeElement[] parameters)
			: base(false, "console.log", new ICodeElement[] { new LiteralValue(text) }.Union(parameters).ToArray()) { }
	}
}
