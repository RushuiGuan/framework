using Albatross.CodeGen.Syntax;
using Albatross.Collections;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class ScopedVariableExpression : SyntaxNode, IExpression, ICodeElement {
		public ScopedVariableExpression(string name) {
			Identifier = new IdentifierNameExpression(name);
		}

		public bool IsConstant { get; init; }
		public IdentifierNameExpression Identifier { get; }
		public ITypeExpression? Type { get; init; }
		public IExpression? Assignment { get; init; }

		public override IEnumerable<ISyntaxNode> Children => new List<ISyntaxNode> { Identifier }.AddIfNotNull(Type, Assignment);

		public override TextWriter Generate(TextWriter writer) {
			if (IsConstant) {
				writer.Append("const ");
			} else {
				writer.Append("let ");
			}
			writer.Code(Identifier);
			if (Type != null) {
				writer.Append(" : ").Code(Type);
			}
			if (Assignment != null) {
				writer.Append(" = ").Code(Assignment);
			}
			writer.Append(";");
			return writer;
		}
	}
}