using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using Albatross.Text;
using System.Reflection;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertPropertyInfoToTypeScriptProperty : IConvertObject<PropertyInfo, Property> {
		ConvertTypeToTypeScriptType convertToTypeScriptType;
		public ConvertPropertyInfoToTypeScriptProperty(ConvertTypeToTypeScriptType convertToTypeScriptType) {
			this.convertToTypeScriptType = convertToTypeScriptType;
		}

		public Property Convert(PropertyInfo from) {
			var property = new Property(from.Name.CamelCase(), convertToTypeScriptType.Convert(from.PropertyType)) {
				Optional = from.PropertyType.GetNullableValueType(out _)
			};
			return property;
		}

		object IConvertObject<PropertyInfo>.Convert(PropertyInfo from) => this.Convert(from);
	}
}
