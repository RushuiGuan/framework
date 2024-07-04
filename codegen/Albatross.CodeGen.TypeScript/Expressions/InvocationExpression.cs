using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class InvocationExpression : SyntaxNode, IExpression {
		public required IIdentifierNameExpression Identifier { get; init; }
		public ListOfSyntaxNodes<IExpression> ArgumentList { get; init; } = new ListOfSyntaxNodes<IExpression>();
		public override IEnumerable<ISyntaxNode> Children => [Identifier, ArgumentList];

		public override TextWriter Generate(TextWriter writer) {
			writer.Code(Identifier).OpenParenthesis().Code(ArgumentList).CloseParenthesis();
			return writer;
		}
	}
}
