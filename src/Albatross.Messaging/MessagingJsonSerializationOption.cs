using System;
using System.Text.Json;

namespace Albatross.Messaging {
	public class MessagingJsonSerializationOption : Albatross.Serialization.IJsonSettings {
		readonly JsonSerializerOptions @default;
		public MessagingJsonSerializationOption() {
			@default = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
			};
		}
		public JsonSerializerOptions Default => @default;
		public JsonSerializerOptions Alternate => throw new NotSupportedException();
	}
}
