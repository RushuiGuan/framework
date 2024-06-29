using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Models {
	public class TypeScriptFile: SyntaxTree ,ICodeElement  {
		// used by import, should not include file extension
		public string Name { get; }

		public TypeScriptFile(string name) {
			this.Name = name;
		}



		public List<EnumDeclaration> EnumDeclarations { get; } = new List<EnumDeclaration>();
		public List<InterfaceDeclaration> InterfaceDeclarations { get; } = new List<InterfaceDeclaration>();
		public List<ClassDeclaration> ClasseDeclarations { get; } = new List<ClassDeclaration>();
		public List<ImportExpression> ImportDeclarations { get; } = new List<ImportExpression>();
		public ISet<string> Artifacts { get; } = new HashSet<string>();
		
		public void BuildArtifacts() {
			Artifacts.Clear();
			EnumDeclarations.ForEach(args => Artifacts.Add(args.Name));
			InterfaceDeclarations.ForEach(args => Artifacts.Add(args.Name));
			ClasseDeclarations.ForEach(args => Artifacts.Add(args.Name));
		}

		public void BuildImports(IEnumerable<TypeScriptFile> dependancies) {
			ImportDeclarations.Clear();
			var types = InterfaceDeclarations.SelectMany(args => args.Properties.Select(args => args.Type))
				.Union(ClasseDeclarations.SelectMany(args => args.Properties.Select(args => args.Type)))
				.Union(ClasseDeclarations.SelectMany(args=>args.Constructor?.Parameters.Select(args=>args.Type) ?? new TypeExpression[0]))
				.Union(ClasseDeclarations.SelectMany(@class => @class.Methods.SelectMany(method => method.Parameters.Select(param => param.Type))))
				.Union(ClasseDeclarations.SelectMany(@class => @class.Methods.Select(method => method.ReturnType)));

			types = types.Union(types.Where(args => args.IsGeneric).SelectMany(args => args.GenericTypeArguments));

			foreach(var file in dependancies) {
				var import = new ImportExpression($"./{file.Name}");
				var referenced = (from artifact in file.Artifacts join type in types on artifact equals type.Name select artifact).ToArray();
				foreach (var item in referenced) {
					import.Items.Add(item);
				}
				if (import.Items.Count > 0) {
					this.ImportDeclarations.Add(import);
				}
			}
		}

		public TextWriter Generate(TextWriter writer) {
			foreach (var item in ImportDeclarations) {
				writer.Code(item);
			}
			writer.WriteLine();
			foreach (var item in EnumDeclarations) {
				writer.Code(item);
			}
			foreach (var item in InterfaceDeclarations) {
				writer.Code(item);
			}
			foreach (var item in ClasseDeclarations) {
				writer.Code(item);
			}
			return writer;
		}
	}
}
