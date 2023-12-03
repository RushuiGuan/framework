﻿using System;
using System.IO;

namespace Albatross.CodeGen.Core {
	public class CodeBlock : ICodeElement {
		public Action<TextWriter> Action { get; set; }

		public CodeBlock(Action<TextWriter> action) {
			this.Action = action;
		}
		public CodeBlock(string content) {
			this.Action = writer => writer.WriteLine(content);
		}
		public CodeBlock() {
			this.Action = _ => { };
		}
		public CodeBlock(ICodeElement codeElement) {
			this.Action = writer => codeElement.Generate(writer);
		}

		public void Add(ICodeElement codeElement) {
			this.Action += writer => codeElement.Generate(writer);
		}

		public void Add(string content) {
			this.Action += (args) => args.WriteLine(content);
		}


		public TextWriter Generate(TextWriter writer) {
			Action(writer);
			return writer;
		}
	}
}