using Albatross.CodeGen.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.TypeScript.Conversion {
	public class ConvertEnumToTypeScriptEnum : IConvertObject<Type, TypeScript.Model.Enum> {
		public TypeScript.Model.Enum Convert(Type type) {
			if (type.IsEnum) {
				var model = new Model.Enum(type.Name) {
					Values = type.GetEnumNames(),
				};
				var attrib = type.GetCustomAttribute<JsonConverterAttribute>();
				model.UseTextValue = attrib?.ConverterType == typeof(JsonStringEnumConverter);
				return model;
			} else {
				throw new InvalidOperationException($"Type {type.Name} is not an Enum type");
			}
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
