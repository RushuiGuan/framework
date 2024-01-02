using System.Linq;

namespace Albatross.CodeGen.TypeScript.Models {
	public class LoggerInfo : MethodCall  {
		public LoggerInfo(string text, params ICodeElement[] parameters)
			: base(false, "this.logger.info", new ICodeElement[] { new LiteralValue(text) }.Union(parameters).ToArray()) { }
	}
}
