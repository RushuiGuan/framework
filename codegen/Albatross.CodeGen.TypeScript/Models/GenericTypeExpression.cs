using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public record class GenericTypeExpression : TypeExpression {
		internal GenericTypeExpression(SyntaxTree syntaxTree) : base(syntaxTree) { }
		public required ArgumentListExpression GenericTypeArguments { get; init; }
		public override TextWriter Generate(TextWriter writer) {
			return writer.Append(this.Name).Append("<").Code(GenericTypeArguments).Append(">");
		}
	}
}
