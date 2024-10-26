using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Albatross.Serialization {
	/// <summary>
	/// Default json serialization option used by webapi and http clients
	///  * default
	///		* camel case property name
	///		* ignore when writing null
	///	 * alternate
	///		* proper case property name
	///		* ignore when writing null
	/// </summary>
	public class DefaultJsonSettings : IJsonSettings {
		public JsonSerializerOptions Default { get; private set; }
		public JsonSerializerOptions Alternate { get; private set; }
		public DefaultJsonSettings() {
			Default = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			};
			Alternate = new JsonSerializerOptions {
				PropertyNamingPolicy = null,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			};
		}
		readonly static Lazy<DefaultJsonSettings> lazy = new Lazy<DefaultJsonSettings>();
		public static DefaultJsonSettings Value => lazy.Value;
	}
}