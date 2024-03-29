﻿using Albatross.Reflection;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Models {
	public class Interface : ICodeElement {
		public Interface(string name, bool isGeneric, IEnumerable<string> genericArgumentTypes) {
			this.Name = name;
			this.IsGeneric = isGeneric;
			this.GenericArgumentTypes = genericArgumentTypes;
			if (IsGeneric) {
				this.Name = name.GetGenericTypeName() + "_";
			}
		}
		public string Name { get; set; }
		public bool IsGeneric { get; set; }
		public IEnumerable<string> GenericArgumentTypes { get; set; }
		public List<Property> Properties { get; set; } = new List<Property>();
		public TypeScriptType? BaseType { get; set; }

		public TextWriter Generate(TextWriter writer) {
			if (IsGeneric && GenericArgumentTypes.Count() == 0) {
				throw new InvalidOperationException($"Missing generic argument for the generic interface: {Name}");
			}
			writer.Append("export ").Append("interface ").Append(Name);
			if (IsGeneric) {
				writer.Append("<").WriteItems(GenericArgumentTypes, ",", (w, item) => w.Append(item)).Append(">");
			}
			if (BaseType != null) {
				writer.Append(" extends ");
				BaseType.Generate(writer);
			}
			using (var scope = writer.BeginScope()) {
				foreach (var property in Properties) {
					scope.Writer.Code(property);
				}
			}
			writer.WriteLine();
			return writer;
		}
	}
}
