using Albatross.CodeGen.Syntax;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class QualifiedIdentifierNameExpression : SyntaxNode, IIdentifierNameExpression {
		public QualifiedIdentifierNameExpression(string name, ISourceExpression source) {
			Identifier = new IdentifierNameExpression(name);
			this.Source = source;
		}
		public IdentifierNameExpression Identifier { get; }
		public ISourceExpression Source { get; }
		public override IEnumerable<ISyntaxNode> Children => [Identifier, Source];

		public override TextWriter Generate(TextWriter writer) {
			writer.Code(Identifier);
			return writer;
		}
	}
}