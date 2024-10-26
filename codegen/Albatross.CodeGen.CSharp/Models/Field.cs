using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.CSharp.Models {
	public class Field : ICodeElement {
		public Field(string name, DotNetType type) {
			this.Name = name;
			this.Type = type;
		}

		public string Name { get; set; }
		public DotNetType Type { get; set; }
		public AccessModifier Modifier { get; set; }
		public bool ReadOnly { get; set; }
		public bool Static { get; set; }
		public bool Const { get; set; }
		public string? Value { get; set; }

		public TextWriter Generate(TextWriter writer) {
			if (Modifier != AccessModifier.None) {
				writer.Code(new AccessModifierElement(Modifier)).Space();
			}
			if (Const) { writer.Const(); }
			if (ReadOnly) { writer.ReadOnly(); }
			if (Static) { writer.Static(); }
			writer.Code(Type).Space().Append(Name);
			if (!string.IsNullOrEmpty(Value)) {
				writer.Append(" = ").Append(Value);
			}
			writer.Semicolon();
			return writer;
		}
	}
}