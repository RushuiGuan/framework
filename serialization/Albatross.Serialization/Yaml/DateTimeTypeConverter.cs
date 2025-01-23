using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Albatross.Serialization.Yaml {
	public class DateTimeTypeConverter : IYamlTypeConverter {
		public const string ISO8601 = "yyyy-MM-ddTHH:mm:ssK";

		public bool Accepts(Type type) {
			return type == typeof(DateTime) || type == typeof(DateTime?);
		}

		public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer) {
			var value = parser.Consume<Scalar>();
			if (type == typeof(DateTime?) && string.IsNullOrEmpty(value.Value)) {
				return null;
			} else {
				return DateTime.ParseExact(value.Value, ISO8601, null);
			}
		}

		public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer) {
			if (value is DateTime dateTime) {
				emitter.Emit(new Scalar(dateTime.ToString(ISO8601)));
			} else if (value is null) {
				emitter.Emit(new Scalar(string.Empty));
			}
		}
	}
}