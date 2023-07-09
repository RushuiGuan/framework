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
		public static readonly JsonSerializerOptions Default = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		};
		public static readonly JsonSerializerOptions Alternate = new JsonSerializerOptions {
			PropertyNamingPolicy = null,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		};

		JsonSerializerOptions IJsonSettings.Default => Default;
		JsonSerializerOptions IJsonSettings.Alternate => Alternate;
	}

	/// <summary>
	/// Formatted json serialization option used by print outs
	/// </summary>
	public class FormattedJsonSettings : IJsonSettings {
		public static readonly JsonSerializerOptions Default = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			WriteIndented = true,
		};
		public static readonly JsonSerializerOptions Alternate = new JsonSerializerOptions {
			PropertyNamingPolicy = null,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			WriteIndented = true,
		};

		JsonSerializerOptions IJsonSettings.Default => Default;
		JsonSerializerOptions IJsonSettings.Alternate => Alternate;
	}

	/// <summary>
	/// Reduced footprint serialization option used by applications only where sender and receiver are both created internally.
	/// </summary>
	public class ReducedFootprintJsonSettings : IJsonSettings {
		public static readonly JsonSerializerOptions Default = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
			WriteIndented = true,
		};
		public static readonly JsonSerializerOptions Alternate = new JsonSerializerOptions {
			PropertyNamingPolicy = null,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
			WriteIndented = true,
		};
		JsonSerializerOptions IJsonSettings.Default => Default;
		JsonSerializerOptions IJsonSettings.Alternate => Alternate;
	}
}
