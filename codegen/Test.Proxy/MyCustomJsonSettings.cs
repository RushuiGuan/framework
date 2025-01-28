using Albatross.Serialization;
using System;
using System.Text.Json;

namespace Test.Proxy {
	public class MyCustomJsonSettings : IJsonSettings {
		public readonly static MyCustomJsonSettings Instance = new MyCustomJsonSettings();
		public JsonSerializerOptions Default { get; } = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			PropertyNameCaseInsensitive = true,
		};
		public JsonSerializerOptions Alternate => throw new NotImplementedException();
	}
}
