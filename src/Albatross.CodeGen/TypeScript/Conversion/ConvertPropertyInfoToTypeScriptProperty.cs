using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.Reflection;

namespace Albatross.CodeGen.TypeScript.Conversion {
	public class ConvertPropertyInfoToTypeScriptProperty : IConvertObject<PropertyInfo, Property> {
		ConvertTypeToTypeScriptType convertToTypeScriptType;
		public ConvertPropertyInfoToTypeScriptProperty(ConvertTypeToTypeScriptType convertToTypeScriptType) {
			this.convertToTypeScriptType = convertToTypeScriptType;
		}

		public Property Convert(PropertyInfo from) {
			var property = new Property {
				Name = from.Name.VariableName(),
				Type = convertToTypeScriptType.Convert(from.PropertyType),
			};
			property.Optional = from.PropertyType.GetNullableValueType(out _);
			return property;
		}

		object IConvertObject<PropertyInfo>.Convert(PropertyInfo from) {
			return this.Convert(from);
		}
	}
}
