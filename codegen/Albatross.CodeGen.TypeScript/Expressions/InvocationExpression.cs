using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class InvocationExpression : SyntaxNode, IExpression {
		public bool UseAwaitOperator { get; init; }
		public required IIdentifierNameExpression Identifier { get; init; }
		public bool Terminate { get; init; }
		public ListOfSyntaxNodes<ITypeExpression> GenericArguments { get; init; } = new ListOfSyntaxNodes<ITypeExpression>();
		public ListOfSyntaxNodes<IExpression> ArgumentList { get; init; } = new ListOfSyntaxNodes<IExpression>();
		public override IEnumerable<ISyntaxNode> Children => [Identifier, ArgumentList];

		public override TextWriter Generate(TextWriter writer) {
			if (UseAwaitOperator) {
				writer.Append("await ");
			}
			writer.Code(Identifier);
			if (GenericArguments.Any()) {
				writer.Append("<").Code(GenericArguments).Append(">");
			}
			writer.OpenParenthesis().Code(ArgumentList).CloseParenthesis();
			if (Terminate) {
				writer.Append(";");
			}
			return writer;
		}
	}
}