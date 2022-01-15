using Albatross.CodeGen.Core;
using Albatross.Reflection;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Model {
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
		public Interface? BaseInterface { get; set; }

		public TextWriter Generate(TextWriter writer) {
			if (IsGeneric && GenericArgumentTypes.Count() == 0) {
				throw new InvalidOperationException($"Missing generic argument for the generic interface: {Name}");
			}
			writer.Append("export ").Append("interface ").Append(Name);
			if (IsGeneric) {
				writer.Append("<").WriteItems(GenericArgumentTypes, ",", (w, item) => w.Append(item)).Append(">");
			}
			if (BaseInterface != null) {
				writer.Append(" extends ").Append(BaseInterface.Name);
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
