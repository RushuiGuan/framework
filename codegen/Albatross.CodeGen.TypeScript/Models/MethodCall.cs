using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class MethodCall : ICodeElement  {
		public MethodCall(bool async, string methodName, params ICodeElement[] parameters) {
			Async = async;
			MethodName = methodName;
			Parameters = parameters;
		}

		public bool Async { get; set; }
		public string MethodName { get; set; }
		public ICodeElement[] Parameters { get; set; }
		public List<TypeScriptType> GenericArguments { get; set; } = new List<TypeScriptType>();

		public TextWriter Generate(TextWriter writer) {
			if (Async) { writer.Append("await "); }
			writer.Append(MethodName);
			if (GenericArguments.Count > 0) {
				writer.Append("<");
				writer.WriteItems(GenericArguments, ",", (w, item) => w.Code(item));
				writer.Append(">");
			}
			writer.OpenParenthesis();
			writer.WriteItems(Parameters, ", ", (w, item) => w.Code(item));
			writer.CloseParenthesis();
			return writer;
		}
	}
}
