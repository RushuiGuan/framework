using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.WebClient;
using CommandLine;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Utility {
	[Verb("typescript-dto")]
	public class GenerateTypeScriptDtoOption : MyUtilityOption { }
	public class GenerateTypeScriptDto : MyUtilityBase<GenerateTypeScriptDtoOption> {
		public GenerateTypeScriptDto(GenerateTypeScriptDtoOption option) : base(option) { }

		public Task<int> RunUtility(Compilation compilation,
			IConvertObject<INamedTypeSymbol, InterfaceDeclaration> interfaceConverter,
			IConvertObject<INamedTypeSymbol, EnumDeclaration> enumConverter) {
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
			var dtoFile = new TypeScriptFileDeclaration("dto") {
				EnumDeclarations = enums.Select(x => enumConverter.Convert(x)).ToList(),
				InterfaceDeclarations = dtoClasses.Select(x => interfaceConverter.Convert(x)).ToList(),
			};
			dtoFile.Generate(System.Console.Out);
			if (!string.IsNullOrEmpty(Options.OutputDirectory)) {
				using (var writer = new StreamWriter(Path.Join(Options.OutputDirectory, dtoFile.FileName))) {
					dtoFile.Generate(writer);
				}
			}
			return Task.FromResult(0);
		}
	}
}
