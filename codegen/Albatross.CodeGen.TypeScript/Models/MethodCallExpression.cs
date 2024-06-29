using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public record class MethodCallExpression :IdentifierNameExpression {
		internal MethodCallExpression(SyntaxTree syntaxTree):base(syntaxTree) { }

		public required ArgumentListExpression ArgumentList { get; init; }

		public override TextWriter Generate(TextWriter writer) {
			writer.Append(Name);
			writer.OpenParenthesis();
			ArgumentList.Generate(writer);
			writer.CloseParenthesis();
			return writer;
		}
	}
}
