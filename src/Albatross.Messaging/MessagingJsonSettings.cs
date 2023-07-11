using System;
using System.Text.Json;

namespace Albatross.Messaging {
	public class MessagingJsonSettings : Serialization.IJsonSettings {
		readonly JsonSerializerOptions @default;
		public MessagingJsonSettings() {
			@default = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
			};
		}
		public JsonSerializerOptions Default => @default;
		public JsonSerializerOptions Alternate => throw new NotSupportedException();
	}
}
