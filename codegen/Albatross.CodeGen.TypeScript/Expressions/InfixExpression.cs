using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public class InfixExpression : IExpression {
		public InfixExpression(string @operator) {
			Operator = @operator;
		}

		public string Operator { get; }
		public bool UseParenthesis { get; init; }
		public required IExpression Left { get; init; }
		public required IExpression Right { get; init; }

		public TextWriter Generate(TextWriter writer) {
			if (UseParenthesis) { writer.OpenParenthesis(); }
			writer.Code(Left).Append(" ").Append(Operator).Append(" ").Code(Right);
			if (UseParenthesis) { writer.CloseParenthesis(); }
			return writer;
		}

		public IEnumerable<ISyntaxNode> GetDescendants() {
			return new ISyntaxNode[] { Left, Right };
		}
	}
}