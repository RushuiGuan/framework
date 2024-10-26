using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class AwaitMethodExpression : SyntaxNode, IExpression {
		public required InvocationExpression MethodCallExpression { get; init; }
		public override IEnumerable<ISyntaxNode> Children => [MethodCallExpression];
		public override TextWriter Generate(TextWriter writer) {
			return writer.Append("await ").Code(MethodCallExpression);
		}
	}
}