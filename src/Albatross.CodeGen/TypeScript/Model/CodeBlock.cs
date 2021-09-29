using System;
using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.Model {
	public class CodeBlock {
		public string? Content { get; set; }
		public List<CodeBlock> Children { get; } = new List<CodeBlock>();
	}
}
