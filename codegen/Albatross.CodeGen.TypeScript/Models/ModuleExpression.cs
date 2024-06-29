using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public record class ModuleExpression : IdentifierNameExpression, ICodeElement {
		internal ModuleExpression(SyntaxTree syntaxTree) : base(syntaxTree) { }
		public override TextWriter Generate(TextWriter writer) {
			return writer.StringLiteral(this.Name, true);
		}
	}
}
