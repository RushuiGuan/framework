using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.WebClient;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeGen.WebClient.Settings;
using Albatross.CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	public class DtoClassInfoModelGenerator : BaseHandler<CodeGenCommandOptions> {
		private Compilation compilation;
		private ConvertClassSymbolToDtoClassModel dtoConverter;
		private readonly ConvertEnumSymbolToDtoEnumModel enumConverter;
		private CodeGenSettings settings;

		public DtoClassInfoModelGenerator(Compilation compilation, ConvertClassSymbolToDtoClassModel dtoConverter,
			ConvertEnumSymbolToDtoEnumModel enumConverter,
			CodeGenSettings settings, 
			IOptions<CodeGenCommandOptions> options) : base(options) {
			this.compilation = compilation;
			this.dtoConverter = dtoConverter;
			this.enumConverter = enumConverter;
			this.settings = settings;
		}

		public override Task<int> InvokeAsync(InvocationContext context) {
			var dtoClasses = new List<INamedTypeSymbol>();
			var enumClasses = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var symbolWalker = new DtoClassEnumWalker(semanticModel, settings.DtoFilter);
				symbolWalker.Visit(syntaxTree.GetRoot());
				dtoClasses.AddRange(symbolWalker.DtoClasses);
				enumClasses.AddRange(symbolWalker.EnumTypes);
			}
			var serializationOptions = new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, };
			var dtoModels = new List<DtoClassInfo>();
			foreach (var item in dtoClasses) {
				if (string.IsNullOrEmpty(options.AdhocFilter) || item.GetFullName().Contains(options.AdhocFilter, System.StringComparison.InvariantCultureIgnoreCase)) {
					var model = dtoConverter.Convert(item);
					dtoModels.Add(model);
				}
			}
			if (dtoModels.Any()) {
				var text = JsonSerializer.Serialize(dtoModels, serializationOptions);
				this.writer.WriteLine(text);
			}
			var enumModels = new List<EnumInfo>();
			foreach (var item in enumClasses) {
				if (string.IsNullOrEmpty(options.AdhocFilter) || item.GetFullName().Contains(options.AdhocFilter, System.StringComparison.InvariantCultureIgnoreCase)) {
					var model = enumConverter.Convert(item);
					enumModels.Add(model);
				}
			}
			if (enumModels.Any()) {
				var text = JsonSerializer.Serialize(enumModels, serializationOptions);
				this.writer.WriteLine(text);
			}

			if (options.OutputDirectory != null) {
				if (dtoModels.Any()) {
					using (var stream = File.OpenWrite(Path.Join(options.OutputDirectory.FullName, "dto.generated.json"))) {
						JsonSerializer.SerializeAsync(stream, dtoModels, serializationOptions);
					}
				}
				if (enumModels.Any()) {
					using (var stream = File.OpenWrite(Path.Join(options.OutputDirectory.FullName, "enum.generated.json"))) {
						JsonSerializer.SerializeAsync(stream, enumModels, serializationOptions);
					}
				}
			}
			return Task.FromResult(0);
		}
	}
}