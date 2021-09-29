using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.CSharp.Model {
	public class Property {
		public Property(string name, DotNetType type) {
			this.Name = name;
			this.Type = type;
		}

		public string Name { get; set; }
		public DotNetType Type { get; set; }
		public AccessModifier Modifier { get; set; } = AccessModifier.Public;
		public AccessModifier SetModifier { get; set; } = AccessModifier.Public;
		public bool Static { get; set; }
		public bool CanWrite { get; set; } = true;
		public bool CanRead { get; set; } = true;
		public CodeBlock? GetCodeBlock { get; set; }
		public CodeBlock? SetCodeBlock { get; set; }
	}
}
