using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class StringInterpolationExpression : SyntaxNode, IExpression {
		public required IExpression[] Expressions { get; init; }

		public override IEnumerable<ISyntaxNode> Children => Expressions;

		public override TextWriter Generate(TextWriter writer) {
			writer.Append("`");
			foreach(var item in Expressions) {
				if (item is StringLiteralExpression literal) {
					writer.Append(literal.Value);
				}else{
					writer.Append("${");
					writer.Code(item);
					writer.Append("}");
				}
			}
			writer.Append("`");
			return writer;
		}
	}
}
