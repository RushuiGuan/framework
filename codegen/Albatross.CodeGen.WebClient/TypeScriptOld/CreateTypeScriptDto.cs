using Albatross.CodeGen.TypeScript.Declarations;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Albatross.CodeGen.WebClient.Settings;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public class CreateTypeScriptDto  {
		private readonly Compilation compilation;
		private readonly CodeGenSettings settings;
		private readonly IConvertObject<INamedTypeSymbol, InterfaceDeclaration> interfaceConverter;
		private readonly IConvertObject<INamedTypeSymbol, EnumDeclaration> enumConverter;

		public CreateTypeScriptDto(Compilation compilation,
			CodeGenSettings settings,
			IConvertObject<INamedTypeSymbol, InterfaceDeclaration> interfaceConverter,
			IConvertObject<INamedTypeSymbol, EnumDeclaration> enumConverter) {
			this.compilation = compilation;
			this.settings = settings;
			this.interfaceConverter = interfaceConverter;
			this.enumConverter = enumConverter;
		}

		public TypeScriptFileDeclaration Generate() {
			var dtoClasses = new List<INamedTypeSymbol>();
			var enums = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var symbolWalker = new DtoClassEnumWalker(semanticModel, settings.CreateTypeScriptDtoFilter());
				symbolWalker.Visit(syntaxTree.GetRoot());
				dtoClasses.AddRange(symbolWalker.DtoClasses);
				enums.AddRange(symbolWalker.EnumTypes);
			}
			return new TypeScriptFileDeclaration("dto") {
				EnumDeclarations = enums.Select(x => enumConverter.Convert(x)).ToList(),
				InterfaceDeclarations = dtoClasses.Select(x => interfaceConverter.Convert(x)).ToList(),
			};
		}
	}
}
