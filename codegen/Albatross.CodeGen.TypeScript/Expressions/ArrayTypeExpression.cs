using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class ArrayTypeExpression : SyntaxNode, ITypeExpression {
		public ITypeExpression Type { get; init; } = Defined.Types.Any();
		public override IEnumerable<ISyntaxNode> Children => [Type];
		public bool Optional { get; init; }

		public override TextWriter Generate(TextWriter writer) {
			if (Type is MultiTypeExpression) {
				return writer.OpenParenthesis().Code(Type).CloseParenthesis().Append("[]");
			} else {
				return writer.Code(Type).Append("[]");
			}
		}
	}
}