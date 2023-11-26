using Albatross.CodeGen.Core;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Albatross.CodeGen.TypeScript.Model {
	public class JsonObject : ICodeElement {
		public JsonObject(IDictionary<string,object> values) {
			Values = values;
		}

		public IDictionary<string, object> Values { get; set; }

		public TextWriter Generate(TextWriter writer) {
			var text = JsonSerializer.Serialize(Values, new JsonSerializerOptions { 
				 PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			});
			writer.Append(text);
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
