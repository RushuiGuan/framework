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

		public override void Build() {
			List<IModuleCodeElement> elements = new List<IModuleCodeElement>();
			foreach (var item in this) {
				item.Build();
				elements.Add(item);
			}
			this.SelectMany<IModuleCodeElement>(x => x);
		}
		public override TextWriter Generate(TextWriter writer) {
			foreach (var item in Imports) {
				writer.Code(item).WriteLine();
			}
			foreach (var item in Classes) {
				writer.Code(item).WriteLine();
			}
			return writer;
		}
	}
}
