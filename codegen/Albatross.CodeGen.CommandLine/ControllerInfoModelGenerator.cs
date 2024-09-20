using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.WebClient;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	public class ControllerInfoModelGenerator : ICommandHandler {
		private readonly Compilation compilation;
		private readonly ConvertApiControllerToControllerModel converter;
		private readonly CodeGenSettings settings;
		private readonly CodeGenCommandOptions options;

		public ControllerInfoModelGenerator(Compilation compilation, ConvertApiControllerToControllerModel converter, 
			CodeGenSettings settings,
			IOptions<CodeGenCommandOptions> options) {
			this.compilation = compilation;
			this.converter = converter;
			this.settings = settings;
			this.options = options.Value;
		}

		public int Invoke(InvocationContext context) {
			throw new System.NotImplementedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			var controllerClass = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var dtoClassWalker = new ApiControllerClassWalker(semanticModel, settings.ControllerFilter);
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				controllerClass.AddRange(dtoClassWalker.Result);
			}
			foreach(var item in controllerClass) {
				if(string.IsNullOrEmpty(options.AdhocFilter) || item.GetFullName().Contains(options.AdhocFilter, System.StringComparison.InvariantCultureIgnoreCase)) {
					var model = converter.Convert(item);
					var text = JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, });
					System.Console.WriteLine(text);
					if (options.OutputDirectory != null) {
						using (var writer = new System.IO.StreamWriter(System.IO.Path.Join(options.OutputDirectory.FullName, $"{item.Name}.generated.json"))) {
							writer.Write(text);
						}
					}
				}
			}
			return Task.FromResult(0);
		}
	}
}