using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Models {
	public record class AwaitMethodExpression : Expression {
		internal AwaitMethodExpression(SyntaxTree syntaxTree) : base(syntaxTree) { }
		public required MethodCallExpression MethodCallExpression { get; init; }
		public override TextWriter Generate(TextWriter writer) {
			return writer.Append("await ").Code(MethodCallExpression);
		}
	}
}
