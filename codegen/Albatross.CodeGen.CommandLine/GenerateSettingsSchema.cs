using Albatross.CommandLine;
using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using Albatross.CodeGen.WebClient.Settings;
using NJsonSchema.Generation;
using Microsoft.Extensions.Options;
using NJsonSchema;

namespace Albatross.CodeGen.CommandLine {
	[Verb("settings-schema", typeof(GenerateSettingsSchema))]
	public record class GenerateSettingsSchemaOptions {
		public FileInfo? File { get; set; }
	}
	public class GenerateSettingsSchema : ICommandHandler {
		private readonly GenerateSettingsSchemaOptions options;

		public GenerateSettingsSchema(IOptions<GenerateSettingsSchemaOptions> options) {
			this.options = options.Value;
		}

		public int Invoke(InvocationContext context) {
			throw new NotImplementedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			var settings = new SystemTextJsonSchemaGeneratorSettings {
				SerializerOptions = new System.Text.Json.JsonSerializerOptions {
					WriteIndented = true,
					PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
				},
			};
			var generator = new JsonSchemaGenerator(settings);
			var schema = generator.Generate(typeof(CodeGenSettings));
			schema.Properties.Add("$schema", new JsonSchemaProperty {
				Type = JsonObjectType.String,
			});
			var text = schema.ToJson();
			System.Console.WriteLine(text);
			if (options.File != null) {
				System.IO.File.WriteAllText(options.File.FullName, text);
			}
			return Task.FromResult(0);
		}
	}
}
