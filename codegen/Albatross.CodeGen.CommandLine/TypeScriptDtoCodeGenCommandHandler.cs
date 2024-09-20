using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.WebClient;
using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	public class TypeScriptDtoCodeGenCommandHandler : ICommandHandler {
		private readonly CodeGenCommandOptions options;
		private readonly Compilation compilation;
		private readonly IConvertObject<INamedTypeSymbol, InterfaceDeclaration> interfaceConverter;
		private readonly IConvertObject<INamedTypeSymbol, EnumDeclaration> enumConverter;
		private readonly CodeGenSettings settings;

		public TypeScriptDtoCodeGenCommandHandler(Compilation compilation,
			IConvertObject<INamedTypeSymbol, InterfaceDeclaration> interfaceConverter,
			IConvertObject<INamedTypeSymbol, EnumDeclaration> enumConverter,
			CodeGenSettings settings,
			IOptions<CodeGenCommandOptions> options) {
			this.options = options.Value;
			this.compilation = compilation;
			this.interfaceConverter = interfaceConverter;
			this.enumConverter = enumConverter;
			this.settings = settings;
		}

		public int Invoke(InvocationContext context) {
			throw new System.NotImplementedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			var dtoClasses = new List<INamedTypeSymbol>();
			var enums = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var dtoClassWalker = new DtoClassEnumWalker(semanticModel, settings.TypeScriptDtoFilter);
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				dtoClasses.AddRange(dtoClassWalker.DtoClasses);

				var enumWalker = new EnumTypeWalker(semanticModel, settings.EnumFilter);
				enumWalker.Visit(syntaxTree.GetRoot());
				enums.AddRange(enumWalker.EnumTypes);
			}
			var dtoFile = new TypeScriptFileDeclaration("dto") {
				EnumDeclarations = enums.Select(x => enumConverter.Convert(x)).ToList(),
				InterfaceDeclarations = dtoClasses.Select(x => interfaceConverter.Convert(x)).ToList(),
			};
			dtoFile.Generate(System.Console.Out);
			if (options.OutputDirectory != null) {
				using (var writer = new StreamWriter(Path.Join(options.OutputDirectory.FullName, dtoFile.FileName))) {
					dtoFile.Generate(writer);
				}
			}
			return Task.FromResult(0);
		}
	}
}