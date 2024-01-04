using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Class : ICodeElement {
		public string Name { get; set; }
		public Class[] BaseClass { get; set; } = Array.Empty<Class>();
		public string Module { get; set; } = string.Empty;

		public Class(string name) {
			Name = name;
		}

		public Constructor Constructor { get; set; } = new Constructor();
		public List<Method> Methods { get; set; } = new List<Method>();
		public List<Property> Properties { get; set; } = new List<Property>();
		public List<Field> Fields { get; set; } = new List<Field>();

		public TextWriter Generate(TextWriter writer) {
			this.Constructor.Fields.AddRange(this.Fields.Where(x => !x.Static));
			writer.Append("class ").Append(Name);
			if (BaseClass.Any()) {
				writer.OpenParenthesis().WriteItems(BaseClass.Select(x => x.Name), ", ").CloseParenthesis();
			}
			using (var scope = writer.BeginPythonScope()) {
				if(Constructor.HasBody) {
					scope.Writer.Code(Constructor);
				}
				var hasStaticFields = false;
				foreach (var item in Fields.Where(x => x.Static)) {
					scope.Writer.Code(item).WriteLine();
					hasStaticFields = true;
				}
				if(hasStaticFields) {
					scope.Writer.WriteLine();
				}
				foreach (var item in Properties) {
					scope.Writer.Code(item).WriteLine();
					scope.Writer.WriteLine();
				}
				foreach (var item in Methods) {
					scope.Writer.Code(item).WriteLine();
					scope.Writer.WriteLine();
				}
			}
			return writer;
		}
	}
}
