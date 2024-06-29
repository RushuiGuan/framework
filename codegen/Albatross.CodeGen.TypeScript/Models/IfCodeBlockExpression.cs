using Albatross.Text;
using System;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class IfCodeBlockExpression : ICodeElement  {
		internal IfCodeBlockExpression() { }

		public required Expression Condition { get; init; }
		public required Expression CodeBlock { get; init; }

		public TextWriter Generate(TextWriter writer) {
			var scope = writer.Append("if (").Append(Condition).Append(")").BeginScope();
			scope.Writer.Code(CodeBlock);
			return writer;
		}
	}
}
