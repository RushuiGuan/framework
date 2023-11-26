using System.Text.Json;

namespace Albatross.Serialization {
	/// <summary>
	/// <see cref="System.Text.Json.JsonSerializerOptions"/> is a complex data type.  Caching and reuse is highly recommended.  
	/// Use the interface below to inject the serialization options.  A static property can also be use instead of dependency injection.
	/// </summary>
	public interface IJsonSettings {
		JsonSerializerOptions Default { get; }
		JsonSerializerOptions Alternate { get; }
	}
}
