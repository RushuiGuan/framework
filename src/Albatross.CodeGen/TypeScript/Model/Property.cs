using Albatross.CodeGen.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Property : ICodeElement{
		public Property(string name, TypeScriptType type) {
			this.Name = name;
			this.Type = type;
		}
		public bool IsPrivate { get; set; }
		public string Name { get; set; }
		public TypeScriptType Type { get; set; }
		public bool Optional { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append(Name);
			if (Optional) { 
				writer.Append("?"); 
			}
			writer.Append(": ").Code(Type).Append(";").WriteLine();
			return writer;
		}
	}
}
