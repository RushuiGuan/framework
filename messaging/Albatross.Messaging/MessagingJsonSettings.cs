using System;
using System.Text.Json;

namespace Albatross.Messaging {
	public class MessagingJsonSettings : Serialization.IJsonSettings {
		public JsonSerializerOptions Default { get; private set; }
		public JsonSerializerOptions Alternate => throw new NotSupportedException();

		public MessagingJsonSettings() {
			Default = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
			};
		}
		static readonly Lazy<MessagingJsonSettings> lazy = new Lazy<MessagingJsonSettings>();
		public static MessagingJsonSettings Value => lazy.Value;
	}
}