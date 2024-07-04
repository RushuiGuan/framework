using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Albatross.Text;
using System.Reflection;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertPropertyInfoToTypeScriptProperty : IConvertObject<PropertyInfo, PropertyDeclaration> {
		ConvertTypeToTypeScriptType convertToTypeScriptType;
		private readonly TypeConverterFactory factory;

		public ConvertPropertyInfoToTypeScriptProperty(ConvertTypeToTypeScriptType convertToTypeScriptType, TypeConverterFactory factory) {
			this.convertToTypeScriptType = convertToTypeScriptType;
			this.factory = factory;
		}

		public PropertyDeclaration Convert(PropertyInfo from) {
			return new PropertyDeclaration(from.Name.CamelCase()) {
				Type = convertToTypeScriptType.Convert(from.PropertyType),
			};
		}

		object IConvertObject<PropertyInfo>.Convert(PropertyInfo from) => this.Convert(from);
	}
}
