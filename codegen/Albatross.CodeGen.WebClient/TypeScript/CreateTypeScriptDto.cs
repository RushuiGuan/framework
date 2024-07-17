using Albatross.CodeGen.TypeScript.Declarations;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public class CreateTypeScriptDto  {
		private readonly Compilation compilation;
		private readonly IConvertObject<INamedTypeSymbol, InterfaceDeclaration> interfaceConverter;
		private readonly IConvertObject<INamedTypeSymbol, EnumDeclaration> enumConverter;

		public CreateTypeScriptDto(Compilation compilation,
			IConvertObject<INamedTypeSymbol, InterfaceDeclaration> interfaceConverter,
			IConvertObject<INamedTypeSymbol, EnumDeclaration> enumConverter) {
			this.compilation = compilation;
			this.interfaceConverter = interfaceConverter;
			this.enumConverter = enumConverter;
		}

		public TypeScriptFileDeclaration Generate() {
			var dtoClasses = new List<INamedTypeSymbol>();
			var enums = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var dtoClassWalker = new DtoClassWalker(semanticModel);
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				dtoClasses.AddRange(dtoClassWalker.Result);

				var enumWalker = new EnumTypeWalker(semanticModel);
				enumWalker.Visit(syntaxTree.GetRoot());
				enums.AddRange(enumWalker.Result);
			}
			return new TypeScriptFileDeclaration("dto") {
				EnumDeclarations = enums.Select(x => enumConverter.Convert(x)).ToList(),
				InterfaceDeclarations = dtoClasses.Select(x => interfaceConverter.Convert(x)).ToList(),
			};
		}
	}
}
