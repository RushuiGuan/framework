using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public abstract record class Expression : ICodeElement {
		internal Expression(SyntaxTree syntaxTree) {
			this.SyntaxTree = syntaxTree;
		}
		public SyntaxTree SyntaxTree { get; }
		public abstract TextWriter Generate(TextWriter writer);
	}
}
