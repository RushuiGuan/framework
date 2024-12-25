using Albatross.CodeGen.WebClient.Settings;
using Albatross.CommandLine;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NJsonSchema.Generation;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	[Verb("settings-schema", typeof(GenerateSettingsSchema))]
	public record class GenerateSettingsSchemaOptions {
		public FileInfo? File { get; set; }
	}
	public class GenerateSettingsSchema : BaseHandler<GenerateSettingsSchemaOptions> {
		public GenerateSettingsSchema(IOptions<GenerateSettingsSchemaOptions> options) : base(options) {
		}

		public override Task<int> InvokeAsync(InvocationContext context) {
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