using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.WebClient.Settings;
using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;

namespace Albatross.CodeGen.CommandLine {
	[Verb("typescript-entrypoint", typeof(TypeScriptEntryPointCodeGenCommandHandler))]
	public record TypeScriptEntryPointCodeGenOptions {
		[Option("o")]
		public DirectoryInfo OutputDirectory { get; init; } = null!;

		[Option("s")]
		public FileInfo? SettingsFile { get; set; }
	}
	public class TypeScriptEntryPointCodeGenCommandHandler : BaseHandler<TypeScriptEntryPointCodeGenOptions> {
		private readonly ILogger<TypeScriptEntryPointCodeGenCommandHandler> logger;
		private readonly TypeScriptWebClientSettings settings;

		public TypeScriptEntryPointCodeGenCommandHandler(IOptions<TypeScriptEntryPointCodeGenOptions> options,
			ILogger<TypeScriptEntryPointCodeGenCommandHandler> logger,
			CodeGenSettings settings) : base(options) {
			this.logger = logger;
			this.settings = settings.TypeScriptWebClientSettings;
		}

		public override int Invoke(InvocationContext context) {
			string entryFile = Path.Combine(this.options.OutputDirectory.FullName, this.settings.EntryFile);
			var sourceFoler = Path.Combine(this.options.OutputDirectory.FullName, this.settings.SourcePathRelatedToEntryFile);

			var entries = new HashSet<string>();
			if (File.Exists(entryFile)) {
				using (var reader = new StreamReader(entryFile)) {
					while (!reader.EndOfStream) {
						var line = reader.ReadLine()?.Trim();
						if (!string.IsNullOrEmpty(line)) {
							entries.Add(line);
						}
					}
				}
			}
			foreach (var file in Directory.GetFiles(sourceFoler, "*.generated.ts", SearchOption.TopDirectoryOnly)) {
				string source = $"./{settings.SourcePathRelatedToEntryFile}/{new FileInfo(file).Name}";
				var exportExpression = new ExportExpression {
					Source = new FileNameSourceExpression(source),
				};
				var writer = new StringWriter();
				exportExpression.Generate(writer);
				entries.Add(writer.ToString().Trim());
			}
			using (var writer = new StreamWriter(entryFile, false)) {
				foreach (var entry in entries) {
					writer.WriteLine(entry);
				}
			}
			return 0;
		}
	}
}