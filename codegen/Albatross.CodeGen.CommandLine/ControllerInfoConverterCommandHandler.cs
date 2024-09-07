using Albatross.CodeGen.Utility;
using Albatross.CodeGen.WebClient.Models;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	public class ControllerInfoConverterCommandHandler : ICommandHandler {
		private readonly Compilation compilation;
		private readonly ConvertApiControllerToControllerInfo converter;
		private readonly ControllerInfoCommandOptions options;

		public ControllerInfoConverterCommandHandler(Compilation compilation, ConvertApiControllerToControllerInfo converter, IOptions<ControllerInfoCommandOptions> options) {
			this.compilation = compilation;
			this.converter = converter;
			this.options = options.Value;
		}

		public int Invoke(InvocationContext context) {
			throw new System.NotImplementedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			var controllerClass = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var dtoClassWalker = new ApiControllerClassWalker(semanticModel);
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				controllerClass.AddRange(dtoClassWalker.Result);
			}
			foreach(var item in controllerClass) {
				if(string.IsNullOrEmpty(options.Controller) || string.Equals(item.Name, options.Controller, System.StringComparison.OrdinalIgnoreCase)) {
					var model = converter.Convert(item);
					System.Console.WriteLine(JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, }));
				}
			}
			return Task.FromResult(0);
		}
	}
}