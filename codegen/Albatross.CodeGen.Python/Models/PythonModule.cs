using Albatross.Collections;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class PythonModule : ICodeElement {
		public string Name { get; set; }
		public PythonModule(string name) {
			Name = name;
		}

		public List<Import> Imports { get; } = new List<Import>();
		public List<Method> Functions { get; } = new List<Method>();
		public List<ClassDeclaration> Classes { get; } = new List<ClassDeclaration>();

		void Build(Dictionary<string, Import> dict, IEnumerable<IModuleCodeElement> elements) {
			foreach (var item in elements) {
				item.Build();
				if (!string.IsNullOrEmpty(item.Module)) {
					if (string.IsNullOrEmpty(item.Name)) {
						var import = dict.GetOrAdd(string.Empty, () => new Import(string.Empty));
						import.Names.Add(item.Module);
					} else {
						var import = dict.GetOrAdd(item.Module, () => new Import(item.Module));
						import.Names.Add(item.Name);
					}
				}
				Build(dict, item);
			}
		}

		public void Build() {
			var dict = new Dictionary<string, Import>();
			Build(dict, Classes);
			Imports.AddRange(dict.Values);
		}

		public TextWriter Generate(TextWriter writer) {
			foreach (var item in Imports) {
				writer.Code(item);
			}
			if (Functions.Any()) { writer.AppendLine(); }
			Functions.Select(x => writer.Code(x));

			if (Classes.Any()) { writer.AppendLine(); }
			Classes.Sort();
			Classes.ForEach(x => writer.Code(x));

			return writer;
		}
	}
}