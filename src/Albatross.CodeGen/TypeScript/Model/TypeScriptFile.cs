using Albatross.CodeGen.Core;
using Albatross.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Model {
	public class TypeScriptFile: ICodeElement  {
		public TypeScriptFile(string name) {
			this.Name = name;
		}
		// used by import, should not include file extension
		public string Name { get; set; }
		public string? Namespace { get; set; }

		public List<Enum> Enums { get; set; } = new List<Enum>();
		public List<Interface> Interfaces { get; set; } = new List<Interface>();
		public List<Class> Classes { get; set; } = new List<Class>();
		public List<Import> Imports { get; set; } = new List<Import>();

		public ISet<string> Artifacts { get; } = new HashSet<string>();
		public void BuildArtifacts() {
			Artifacts.Clear();
			Enums.ForEach(args => Artifacts.Add(args.Name));
			Interfaces.ForEach(args => Artifacts.Add(args.Name));
			Classes.ForEach(args => Artifacts.Add(args.Name));
		}

		public void BuildImports(IEnumerable<TypeScriptFile> dependancies) {
			Imports.Clear();
			var types = Interfaces.SelectMany(args => args.Properties.Select(args => args.Type))
				.Union(Classes.SelectMany(args => args.Properties.Select(args => args.Type)))
				.Union(Classes.SelectMany(args=>args.Constructor?.Parameters.Select(args=>args.Type) ?? new TypeScriptType[0]))
				.Union(Classes.SelectMany(@class => @class.Methods.SelectMany(method => method.Parameters.Select(param => param.Type))));

			dependancies.ForEach(args => Imports.Add(new Import(args)));

			foreach(var type in types) {
				foreach(var import in Imports) {
					if (import.From.Artifacts.Contains(type.Name)) {
						import.Items.Add(type.Name);
						continue;
					}
				}
			}
		}

		public TextWriter Generate(TextWriter writer) {
			foreach (var item in Imports) {
				writer.Code(item);
			}
			foreach (var item in Enums) {
				writer.Code(item);
			}
			foreach (var item in Interfaces) {
				writer.Code(item);
			}
			foreach (var item in Classes) {
				writer.Code(item);
			}
			return writer;
		}
	}
}
