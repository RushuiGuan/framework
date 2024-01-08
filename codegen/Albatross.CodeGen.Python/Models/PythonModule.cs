using Albatross.Collections;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class PythonModule : CompositeModuleCodeElement {
		public PythonModule(string name) : base(name, string.Empty) { }

		public IEnumerable<Import> Imports => Collection<Import>(nameof(Imports));
		public void AddImport(Import import) => AddCodeElement(import, nameof(Imports));
		public void RemoveImport(Import import) => RemoveCodeElement(import, nameof(Imports));

		public IEnumerable<Class> Classes => Collection<Class>(nameof(Classes));
		public void AddClass(Class @class) => AddCodeElement(@class, nameof(Classes));
		public void RemoveClass(Class @class) => RemoveCodeElement(@class, nameof(Classes));

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

		public override void Build() {
			var dict = new Dictionary<string, Import>();
			Build(dict, this);
			dict.Values.ForEach(x => AddImport(x));
		}

		public override TextWriter Generate(TextWriter writer) {
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
