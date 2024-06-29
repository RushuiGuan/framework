using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public record class JsonPropertyExpression :IdentifierNameExpression, ICodeElement {
		internal JsonPropertyExpression(SyntaxTree syntaxTree) : base(syntaxTree) { }

		public required Expression Expression { get; init; }

		public override TextWriter Generate(TextWriter writer) {
			if (Expression is IdentifierNameExpression identifier && identifier.Name == this.Name) {
				writer.Append(this.Name);
			} else {
				writer.Append(this.Name).Append(": ").Code(Expression);
			}
			return writer;
		}
	}
}
