using Albatross.CodeGen.Syntax;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class EmptyExpression : SyntaxNode, IExpression {
		public override IEnumerable<ISyntaxNode> Children => [];
		public override TextWriter Generate(TextWriter writer) {
			return writer;
		}
	}
}