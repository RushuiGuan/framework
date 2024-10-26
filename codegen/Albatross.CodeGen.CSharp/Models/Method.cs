using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.CSharp.Models {
	public class Method : ICodeElement {
		public Method(string name) {
			Name = name;
		}

		public DotNetType ReturnType { get; set; } = DotNetType.Void();
		public string Name { get; set; }
		public AccessModifier AccessModifier { get; set; }
		public IEnumerable<Parameter> Parameters { get; set; } = new Parameter[0];
		public bool Async { get; set; }
		public bool Static { get; set; }
		public bool Virtual { get; set; }
		public bool Override { get; set; }
		public ICodeElement CodeBlock { get; set; } = new CodeBlock();

		public virtual TextWriter Generate(TextWriter writer) {
			writer.Code(new AccessModifierElement(AccessModifier)).Space();
			if (Static) {
				writer.Static();
			} else if (Override) {
				writer.Write("override ");
			} else if (Virtual) {
				writer.Write("virtual ");
			}
			if (Async) { writer.Write("async "); }
			writer.Code(ReturnType).Space().Append(Name).OpenParenthesis()
				.Code(new ParameterCollection(Parameters))
				.CloseParenthesis();
			using (var scope = writer.BeginScope()) {
				scope.Writer.Code(CodeBlock);
			}
			return writer;
		}
	}
}