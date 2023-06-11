using System.Text.Json;
using System.Text.Json.Serialization;

namespace Albatross.Serialization {
	/// <summary>
	/// <see cref="System.Text.Json.JsonSerializerOptions"/> is a complex data type.  Caching and reuse is highly recommended.  
	/// Use the interface below to inject the serialization options.  A static property can also be use instead of dependency injection.
	/// </summary>
	public interface IJsonSerializationOption {
		JsonSerializerOptions Default { get; }
		JsonSerializerOptions Alternate { get; }
	}
	/// <summary>
	/// Default json serialization option used by webapi and http clients
	/// </summary>
	public class DefaultJsonSerializationOption : IJsonSerializationOption {
		public DefaultJsonSerializationOption() {
			Default = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			};
			Alternate = new JsonSerializerOptions {
				PropertyNamingPolicy = null,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			};
		}
		public JsonSerializerOptions Default { get; }
		public JsonSerializerOptions Alternate { get; }
	}

	/// <summary>
	/// Formatted json serialization option used by print outs
	/// </summary>
	public class FormattedJsonSerializationOption : IJsonSerializationOption {
		public FormattedJsonSerializationOption() {
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
		public JsonSerializerOptions Default { get; }
		public JsonSerializerOptions Alternate { get; }
	}

	/// <summary>
	/// Reduced footprint serialization option used by applications only where sender and receiver are both created internally.
	/// </summary>
	public class ReducedFootprintJsonSerializationOption : IJsonSerializationOption {
		public ReducedFootprintJsonSerializationOption() {
			Default = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
			};
			Alternate = new JsonSerializerOptions {
				PropertyNamingPolicy = null,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
			};
		}
		public JsonSerializerOptions Default { get; }
		public JsonSerializerOptions Alternate { get; }
	}
}
