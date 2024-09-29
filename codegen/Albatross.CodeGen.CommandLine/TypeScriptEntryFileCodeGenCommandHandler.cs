﻿using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.WebClient.Settings;
using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	[Verb("typescript-entry-file", typeof(TypeScriptEntryFileCodeGenCommandHandler))]
	public record TypeScriptEntryFileCodeGenOptions {
		[Option(Alias = ["o"])]
		public DirectoryInfo OutputDirectory { get; init; } = null!;
		
		[Option(Alias = ["s"])]
		public FileInfo? SettingsFile { get; set; }
	}
	public class TypeScriptEntryFileCodeGenCommandHandler : ICommandHandler {
		private readonly ILogger<TypeScriptEntryFileCodeGenCommandHandler> logger;
		private readonly TypeScriptEntryFileCodeGenOptions options;
		private readonly TypeScriptWebClientSettings settings;

		public TypeScriptEntryFileCodeGenCommandHandler(IOptions<TypeScriptEntryFileCodeGenOptions> options, 
			ILogger<TypeScriptEntryFileCodeGenCommandHandler> logger, 
			CodeGenSettings settings) {
			this.options = options.Value;
			this.logger = logger;
			this.settings = settings.TypeScriptWebClientSettings;
		}

		public int Invoke(InvocationContext context) {
			throw new System.NotSupportedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			string entryFile = Path.Combine(this.options.OutputDirectory.FullName, this.settings.EntryFile);
			var sourceFoler = Path.Combine(this.options.OutputDirectory.FullName, this.settings.SourcePathRelatedToEntryFile);
			using (var writer = new StreamWriter(entryFile)) {
				foreach (var file in Directory.GetFiles(sourceFoler, "*.generated.ts", SearchOption.TopDirectoryOnly)) {
					string source = $"./{settings.SourcePathRelatedToEntryFile}/{new FileInfo(file).Name}";
					var exportExpression = new ExportExpression {
						Source = new FileNameSourceExpression(source),
					};
					writer.Code(exportExpression);
					Console.Out.Code(exportExpression);
				}
			}
			return Task.FromResult(0);
		}
	}
}