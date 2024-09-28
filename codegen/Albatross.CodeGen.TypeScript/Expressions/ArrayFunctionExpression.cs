using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class ArrayFunctionExpression : SyntaxNode, IExpression, ICodeElement {
		public ListOfSyntaxNodes<IIdentifierNameExpression> Arguments { get; init; } = new();
		public IExpression Body { get; init; } = new EmptyExpression();

		public override IEnumerable<ISyntaxNode> Children => new List<ISyntaxNode> { Arguments, Body };

		public override TextWriter Generate(TextWriter writer) {
			if (Arguments.Count == 1) {
				writer.Code(Arguments);
			} else {
				writer.OpenParenthesis().Code(Arguments).CloseParenthesis();
			}
			writer.Append(" => ").Code(Body);
			return writer;
		}
	}
}