using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class PythonModule : ICodeElement {
		public List<IHasModule> Dependencies { get; set; } = new List<IHasModule>();

		public string Name { get; set; }
		public PythonModule(string name) {
			Name = name;
		}
		public List<Class> Classes { get; set; } = new List<Class>();


		Import[] BuildImports() {
			var items = this.Classes.SelectMany(x => x.BaseClass).Cast<IHasModule>()
				.Union(this.Classes.SelectMany(x => x.Fields.Select(y => y.Type)).Cast<IHasModule>())
				.Union(this.Classes.SelectMany(x => x.Fields.Select(y => y.Type.DefaultValue)).Where(x=>x is IHasModule).Cast<IHasModule>())
				.Union(this.Classes.SelectMany(x => x.Properties.Select(y => y.Type)).Cast<IHasModule>())
				.Union(this.Classes.SelectMany(x => x.Methods.SelectMany(y => y.Parameters.Select(y => y.Type))).Cast<IHasModule>())
				.Union(this.Classes.SelectMany(x => x.Methods.Select(y => y.ReturnType)).Cast<IHasModule>())
				.Union(this.Classes.SelectMany(x => x.Methods.SelectMany(y => y.Decorators)).Cast<IHasModule>())
				.Union(this.Classes.SelectMany(x => x.Decorators).Cast<IHasModule>())
				.Union(this.Classes.SelectMany(x => x.Constructor.Parameters.Select(y => y.Type)).Cast<IHasModule>())
				.Union(this.Dependencies);

			var result = items
				.Where(x=>!string.IsNullOrEmpty(x.Module))
				.GroupBy(x => x.Module).Select(x => new Import(x.Key, x.Select(y => y.Name)));
			return result.ToArray();
		}

		public TextWriter Generate(TextWriter writer) {
			var imports = BuildImports();
			if (imports.Any()) {
				foreach (var item in imports) {
					writer.Code(item).WriteLine();
				}
				writer.WriteLine();
			}
			foreach(var item in Classes) {
				item.Generate(writer);
			}
			return writer;
		}
	}
}
