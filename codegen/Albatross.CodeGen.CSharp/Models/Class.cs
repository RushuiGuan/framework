using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Models {
	public class Class : ICodeElement {
		public Class(string name) {
			Name = name;
		}

		public AccessModifier AccessModifier { get; set; }
		public string Name { get; set; }
		public Class? BaseClass { get; set; }
		public bool Static { get; set; }
		public bool Sealed { get; set; }
		public bool Abstract { get; set; }
		public bool Partial { get; set; }
		public string? Namespace { get; set; }
		public bool IsGeneric { get; set; }
		public bool Record { get; set; }

		public IEnumerable<string> Imports { get; set; } = new string[0];
		public IEnumerable<Constructor> Constructors { get; set; } = new Constructor[0];
		public IEnumerable<Property> Properties { get; set; } = new Property[0];
		public IEnumerable<Field> Fields { get; set; } = new Field[0];
		public IEnumerable<Method> Methods { get; set; } = new Method[0];
		public IEnumerable<MethodCall> Attributes { get; set; } = new MethodCall[0];
		public bool UseNullablePreprocessor { get; set; }

		public TextWriter Generate(TextWriter writer) {
			if (Imports?.Count() > 0) {
				foreach (var item in Imports) {
					writer.Append("using ").Append(item).AppendLine(";");
				}
				writer.WriteLine();
			}
			if (UseNullablePreprocessor) { writer.AppendLine("#nullable enable"); }
			using (var scope = writer.BeginScope($"namespace {Namespace}")) {
				scope.Writer.WriteAttributes(this.Attributes);
				scope.Writer.Code(new AccessModifierElement(AccessModifier));
				if (Static) { scope.Writer.Append(" static"); }
				if (Sealed) { scope.Writer.Append(" sealed"); }
				if (Record) { scope.Writer.Append(" record"); }
				if (Partial) { scope.Writer.Append(" partial"); }
				scope.Writer.Append(" class ").Append(Name);
				if (BaseClass != null) {
					scope.Writer.Append(" : ").Append(BaseClass.Name);
				}

				using (var childScope = scope.Writer.BeginScope()) {
					if (Constructors?.Count() > 0) {
						foreach (var constructor in Constructors) {
							childScope.Writer.Code(constructor).WriteLine();
						}
					}
					if (Fields?.Count() > 0) {
						foreach (var field in Fields) {
							childScope.Writer.Code(field).WriteLine();
						}
					}
					if (Properties?.Count() > 0) {
						foreach (var property in Properties) {
							childScope.Writer.Code(property).WriteLine();
						}
					}
					if (Methods?.Count() > 0) {
						foreach (var method in Methods) {
							childScope.Writer.Code(method).WriteLine();
						}
					}
				}
			}
			if (UseNullablePreprocessor) {
				writer.WriteLine();
				writer.AppendLine("#nullable disable");
			}
			return writer;
		}
	}
}