using Albatross.CodeGen.Syntax;
using Albatross.Collections;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class IfElseCodeBlockExpression : SyntaxNode, IExpression {
		public IfElseCodeBlockExpression() { }

		public required IExpression Condition { get; init; }
		public IExpression CodeBlock { get; init; } = new EmptyExpression();
		public IExpression? ElseBlock { get; init; }

		public override IEnumerable<ISyntaxNode> Children => new List<ISyntaxNode> {
			Condition, CodeBlock
		}.AddIfNotNull(ElseBlock);

		public override TextWriter Generate(TextWriter writer) {
			using (var mainScope = writer.Append("if ").OpenParenthesis().Append(Condition).CloseParenthesis().BeginScope()) {
				mainScope.Writer.Code(CodeBlock);
			}
			if (ElseBlock != null) {
				using (var elseScope = writer.Append(" else ").BeginScope()) {
					elseScope.Writer.Code(ElseBlock);
				}
			}
			return writer;
		}
	}
}