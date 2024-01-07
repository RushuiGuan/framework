using System;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen {
	public class ModuleCodeBlock : List<IModuleCodeElement>, IModuleCodeElement {
		Action<TextWriter> action;
		public string Name { get; set; } = string.Empty;
		public string Module { get; set; } = string.Empty;
		public string Tag { get; set; } = string.Empty;

		public ModuleCodeBlock() {
			action = _ => { };
		}
		public ModuleCodeBlock(string content) {
			action = writer => writer.WriteLine(content);
		}
		public ModuleCodeBlock(Action<TextWriter> action) {
			this.action = action;
		}
		public ModuleCodeBlock(IModuleCodeElement codeElement) : this(writer => writer.Code(codeElement)) {
			Add(codeElement);
		}

		public void AddCodeBlock(IModuleCodeElement codeElement) {
			action += writer => writer.Code(codeElement);
			Add(codeElement);
		}
		public void AddCodeBlock(string content) {
			action += (args) => args.WriteLine(content);
		}

		public TextWriter Generate(TextWriter writer) {
			action(writer);
			return writer;
		}

		public void Build() { }
	}
}