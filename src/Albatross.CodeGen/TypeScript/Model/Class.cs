using Albatross.CodeGen.Core;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Class : ICodeElement {
		public Class(string name) {
			this.Name = name;
		}
		public string Name { get; set; }

		public AccessModifier AccessModifier { get; set; }
		public Class? BaseClass { get; set; }
		public bool IsGeneric { get; set; }
		public MethodCall? Decorator { get; set; }

		public Constructor? Constructor { get; set; }
		public List<Import> Imports { get; init; } = new List<Import>();
		public List<Getter> Getters { get; set; } = new List<Getter>();
		public List<Property> Properties { get; init; } = new List<Property>();
		public List<Method> Methods { get; init; } = new List<Method>();

		public TextWriter Generate(TextWriter writer) {
			if (this.Decorator != null) { writer.Code(this.Decorator).WriteLine(); }
			writer.Append("export ").Append("class ").Append(Name);
			if(BaseClass != null) {
				writer.Append(" extends ").Append(BaseClass.Name);
			}
			using (var scope = writer.BeginScope()) {
				foreach (var getter in Getters) {
					scope.Writer.Code(getter);
				}
				foreach (var property in Properties) {
					scope.Writer.Code(property);
				}
				if (Constructor != null) {
					scope.Writer.Code(Constructor);
				}
				foreach (var method in Methods) {
					scope.Writer.Code(method);
				}
			}
			writer.WriteLine();
			return writer;
		}
	}
}
