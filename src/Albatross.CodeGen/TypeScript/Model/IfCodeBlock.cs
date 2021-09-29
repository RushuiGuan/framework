using System;
namespace Albatross.CodeGen.TypeScript.Model {
	public class IfCodeBlock : CodeBlock {
		public string Expression { get; set; }

		public IfCodeBlock(string expression) {
			this.Expression = expression;
		}
	}
}
