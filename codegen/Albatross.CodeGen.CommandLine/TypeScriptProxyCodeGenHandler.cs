﻿using Albatross.CodeGen.Utility;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeGen.WebClient.TypeScript;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	public class TypeScriptProxyCodeGenHandler : ICommandHandler {
		private readonly ILogger<TypeScriptProxyCodeGenHandler> logger;
		private readonly CodeGenCommandOptions options;
		private readonly Compilation compilation;
		private readonly ConvertApiControllerToWebApi convertToWebApi;
		private readonly ConvertWebApiToTypeScriptFile converToTypeScriptFile;

		public TypeScriptProxyCodeGenHandler(IOptions<CodeGenCommandOptions> options, ILogger<TypeScriptProxyCodeGenHandler> logger, Compilation compilation, ConvertApiControllerToWebApi convertToWebApi, ConvertWebApiToTypeScriptFile converToTypeScriptFile) {
			this.options = options.Value;
			this.logger = logger;
			this.compilation = compilation;
			this.convertToWebApi = convertToWebApi;
			this.converToTypeScriptFile = converToTypeScriptFile;
		}

		public int Invoke(InvocationContext context) {
			throw new System.NotSupportedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			var controllerClass = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var dtoClassWalker = new ApiControllerClassWalker(semanticModel);
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				controllerClass.AddRange(dtoClassWalker.Result);
			}
			foreach (var controller in controllerClass) {
				logger.LogInformation("Generating proxy for {controller}", controller.Name);
				var webApi = this.convertToWebApi.Convert(controller);
				var file = this.converToTypeScriptFile.Convert(webApi);
				file.Generate(System.Console.Out);
				if (options.OutputDirectory != null) {
					using (var writer = new System.IO.StreamWriter(System.IO.Path.Join(options.OutputDirectory.FullName, file.FileName))) {
						file.Generate(writer);
					}
				}
			}
			return Task.FromResult(0);
		}
	}
}