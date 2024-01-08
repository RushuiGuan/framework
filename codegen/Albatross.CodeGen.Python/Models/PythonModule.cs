using Albatross.Collections;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class PythonModule  : ICodeElement{
		public string Name { get; set; }
		public PythonModule(string name) {
			Name = name;
		}

		public List<Import> Imports { get;  } = new List<Import>();
		public List<Class> Classes { get; } = new List<Class>();

		void Build(Dictionary<string, Import> dict, IEnumerable<IModuleCodeElement> elements) {
			foreach (var item in elements) {
				item.Build();
				if (!string.IsNullOrEmpty(item.Module)) {
					var import = dict.GetOrAdd(item.Module, () => new Import(item.Module));
					import.Names.Add(item.Name);
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
			if (Classes.Any()) { writer.AppendLine(); }
			foreach (var item in Classes) {
				writer.Code(item);
			}
			return writer;
		}
	}
}
