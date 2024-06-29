using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Models {
	public record class IdentifierNameExpression : Expression, ICodeElement {
		internal IdentifierNameExpression(SyntaxTree syntaxTree):base(syntaxTree) { }
		public required string Name { get; init; }
		public override TextWriter Generate(TextWriter writer) {
			writer.Append(Name);
			return writer;
		}
	}
}
