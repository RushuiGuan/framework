using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Albatross.Serialization {
	/// <summary>
	/// Formatted json serialization option used by print outs
	/// </summary>
	public class FormattedJsonSettings : IJsonSettings {
		public JsonSerializerOptions Default { get; private set; }
		public JsonSerializerOptions Alternate { get; private set; }
		public FormattedJsonSettings() {
			Default = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				WriteIndented = true,
			};
			Alternate = new JsonSerializerOptions {
				PropertyNamingPolicy = null,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				WriteIndented = true,
			};
		}
		readonly static Lazy<FormattedJsonSettings> lazy = new Lazy<FormattedJsonSettings>();
		public static FormattedJsonSettings Value => lazy.Value;
	}
}