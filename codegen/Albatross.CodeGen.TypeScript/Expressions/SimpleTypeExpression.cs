using Albatross.CodeGen.Syntax;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class SimpleTypeExpression : SyntaxNode, ITypeExpression {
		public required IIdentifierNameExpression Identifier { get; init; }
		public override IEnumerable<ISyntaxNode> Children => [Identifier];
		public bool Optional { get; init; }

		public override TextWriter Generate(TextWriter writer) => writer.Code(Identifier);
	}
}