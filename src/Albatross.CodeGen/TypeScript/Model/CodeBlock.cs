using System;
namespace Albatross.CodeGen.TypeScript.Model {
	public class CodeBlock {
		public CodeBlock() { }
		public CodeBlock(string content) {
			this.Content = content;
		}
		public string Content { get; set; }
	}
}
