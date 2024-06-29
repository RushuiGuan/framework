using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public record class ArrayTypeExpression : Expression{
		internal ArrayTypeExpression(SyntaxTree syntaxTree) : base(syntaxTree) { }
		public required TypeExpression ElementType { get; init; }
		public override TextWriter Generate(TextWriter writer) {
			return writer.Code(ElementType).Append("[]");
		}
	}
}
