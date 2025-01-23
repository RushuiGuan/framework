#if NET6_0_OR_GREATER
using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Albatross.Serialization.Yaml {
	public class DateOnlyTypeConverter : IYamlTypeConverter {
		const string DateFormat = "yyyy-MM-dd";

		public bool Accepts(Type type) {
			return type == typeof(DateOnly) || type == typeof(DateOnly?);
		}

		public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer) {
			var value = parser.Consume<Scalar>();
			if (type == typeof(DateOnly?) && string.IsNullOrEmpty(value.Value)) {
				return null;
			} else {
				return DateOnly.ParseExact(value.Value, DateFormat, null);
			}
		}

		public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer) {
			if (value is DateOnly date) {
				emitter.Emit(new Scalar(date.ToString(DateFormat)));
			} else if (value is null) {
				emitter.Emit(new Scalar(string.Empty));
			}
		}
	}
}
#endif