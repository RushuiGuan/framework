using System;
using System.IO;

namespace Albatross.CodeGen {
	public class CodeBlock : ICodeElement {
		public Action<TextWriter> Action { get; set; }

		public CodeBlock(Action<TextWriter> action) {
			Action = action;
		}
		public CodeBlock(string content) {
			Action = writer => writer.WriteLine(content);
		}
		public CodeBlock() {
			Action = _ => { };
		}
		public CodeBlock(ICodeElement codeElement) {
			Action = writer => codeElement.Generate(writer);
		}

		public void Add(ICodeElement codeElement) {
			Action += writer => codeElement.Generate(writer);
		}

		public void Add(string content) {
			Action += (args) => args.WriteLine(content);
		}


		public TextWriter Generate(TextWriter writer) {
			Action(writer);
			return writer;
		}
	}
}