using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Albatross.Serialization {
	/// <summary>
	/// Reduced footprint serialization option used by applications only where sender and receiver are both created internally.
	/// * default
	///		* camel case
	///		* ignore when writing default
	/// * alternate
	///		* proper case
	///		* ignore when writing default
	/// </summary>
	public class ReducedFootprintJsonSettings : IJsonSettings {
		public JsonSerializerOptions Default { get; private set; }
		public JsonSerializerOptions Alternate { get; private set; }
		public ReducedFootprintJsonSettings() {
			Default = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
			};
			Alternate = new JsonSerializerOptions {
				PropertyNamingPolicy = null,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
			};
		}
		readonly static Lazy<ReducedFootprintJsonSettings> lazy = new Lazy<ReducedFootprintJsonSettings>();
		public static ReducedFootprintJsonSettings Value => lazy.Value;
	}
}