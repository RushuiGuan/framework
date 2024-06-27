using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Albatross.CodeGen.TypeScript.Models {
	public class JsonObject : ICodeElement {
		List<JsonProperty> properties = new List<JsonProperty>();

		public JsonObject Add(string property, ICodeElement expression) {
			properties.Add(new JsonProperty(property, expression));
			return this;
		}
		
		public TextWriter Generate(TextWriter writer) {
			if (properties.Any()) {
				writer.Append("{ ").WriteItems(this.properties, ", ", (w, x) => w.Code(x), " ", " ").Append(" }");
			} else {
				writer.Append("{}");
			}
			return writer;
		}
	}

	public class JsonObject<T> : ICodeElement {
		public JsonObject(T value) {
			Value = value;
		}

		public T Value { get; set; }

		public TextWriter Generate(TextWriter writer) {
			var text = JsonSerializer.Serialize<T>(Value, new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			});
			writer.Append(text);
			return writer;
		}
	}
}
