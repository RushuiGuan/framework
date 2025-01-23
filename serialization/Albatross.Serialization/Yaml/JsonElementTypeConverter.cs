using System;
using System.Text.Json;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Albatross.Serialization.Yaml {
	public class JsonElementTypeConverter : IYamlTypeConverter {
		public bool Accepts(Type type) => type == typeof(JsonElement);

		public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer) {
			throw new NotSupportedException();
		}

		public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer) {
			if (value is JsonElement jsonElement) {
				if (jsonElement.ValueKind == JsonValueKind.String) {
					emitter.Emit(new Scalar(null, null, jsonElement.GetString() ?? string.Empty, ScalarStyle.SingleQuoted, true, false));
				} else {
					emitter.Emit(new Scalar(jsonElement.GetRawText()));
				}
			} else if (value is null) {
				emitter.Emit(new Scalar(string.Empty));
			}
		}
	}
}