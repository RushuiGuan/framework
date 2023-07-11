using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Albatross.Serialization {
	/// <summary>
	/// <see cref="System.Text.Json.JsonSerializerOptions"/> is a complex data type.  Caching and reuse is highly recommended.  
	/// Use the interface below to inject the serialization options.  A static property can also be use instead of dependency injection.
	/// </summary>
	public interface IJsonSettings {
		JsonSerializerOptions Default { get; }
		JsonSerializerOptions Alternate { get; }
	}
	/// <summary>
	/// Default json serialization option used by webapi and http clients
	/// </summary>
	public class DefaultJsonSettings : IJsonSettings {
		private static readonly Lazy<JsonSerializerOptions> @default = new Lazy<JsonSerializerOptions>(()=> new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		});

		private static readonly Lazy<JsonSerializerOptions> alternate = new Lazy<JsonSerializerOptions>(() => new JsonSerializerOptions {
			PropertyNamingPolicy = null,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		});
		public static JsonSerializerOptions Default => @default.Value ;
		public static JsonSerializerOptions Alternate => alternate.Value;

		JsonSerializerOptions IJsonSettings.Default => @default.Value;
		JsonSerializerOptions IJsonSettings.Alternate => alternate.Value;
	}

	/// <summary>
	/// Formatted json serialization option used by print outs
	/// </summary>
	public class FormattedJsonSettings : IJsonSettings {
		private static readonly Lazy<JsonSerializerOptions> @default = new Lazy<JsonSerializerOptions>(() => new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			WriteIndented = true,
		});

		private static readonly Lazy<JsonSerializerOptions> alternate = new Lazy<JsonSerializerOptions>(() => new JsonSerializerOptions {
			PropertyNamingPolicy = null,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			WriteIndented = true,
		});
		public static JsonSerializerOptions Default => @default.Value;
		public static JsonSerializerOptions Alternate => alternate.Value;

		JsonSerializerOptions IJsonSettings.Default => @default.Value;
		JsonSerializerOptions IJsonSettings.Alternate => alternate.Value;
	}

	/// <summary>
	/// Reduced footprint serialization option used by applications only where sender and receiver are both created internally.
	/// </summary>
	public class ReducedFootprintJsonSettings : IJsonSettings {
		private static readonly Lazy<JsonSerializerOptions> @default = new Lazy<JsonSerializerOptions>(() => new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		});

		private static readonly Lazy<JsonSerializerOptions> alternate = new Lazy<JsonSerializerOptions>(() => new JsonSerializerOptions {
			PropertyNamingPolicy = null,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		});
		public static JsonSerializerOptions Default => @default.Value;
		public static JsonSerializerOptions Alternate => alternate.Value;

		JsonSerializerOptions IJsonSettings.Default => @default.Value;
		JsonSerializerOptions IJsonSettings.Alternate => alternate.Value;
	}
}
