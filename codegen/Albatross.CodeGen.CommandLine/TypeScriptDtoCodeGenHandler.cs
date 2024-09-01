using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.WebClient;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	public class TypeScriptDtoCodeGenHandler : ICommandHandler {
		private readonly CodeGenCommandOptions options;
		private readonly Compilation compilation;
		private readonly IConvertObject<INamedTypeSymbol, InterfaceDeclaration> interfaceConverter;
		private readonly IConvertObject<INamedTypeSymbol, EnumDeclaration> enumConverter;

		public TypeScriptDtoCodeGenHandler(Compilation compilation,
			IConvertObject<INamedTypeSymbol, InterfaceDeclaration> interfaceConverter,
			IConvertObject<INamedTypeSymbol, EnumDeclaration> enumConverter,
			IOptions<CodeGenCommandOptions> options) {
			this.options = options.Value;
			this.compilation = compilation;
			this.interfaceConverter = interfaceConverter;
			this.enumConverter = enumConverter;
		}

		public int Invoke(InvocationContext context) {
			throw new System.NotImplementedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
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
			if (options.OutputDirectory != null) {
				using (var writer = new StreamWriter(Path.Join(options.OutputDirectory.FullName, dtoFile.FileName))) {
					dtoFile.Generate(writer);
				}
			}
			return Task.FromResult(0);
		}
	}
}