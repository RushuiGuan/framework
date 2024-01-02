using Albatross.CodeGen.TypeScript.Models;
using System;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertTypeToTypeScriptClass : IConvertObject<Type, TypeScript.Models.Class> {
		ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty;
		public ConvertTypeToTypeScriptClass(ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty) {
			this.convertPropertyInfoToTypeScriptProperty = convertPropertyInfoToTypeScriptProperty;
		}
		public TypeScript.Models.Class Convert(Type type) {
			return new Class(type.Name) {
				Properties = (from property in type.GetProperties() 
							  select convertPropertyInfoToTypeScriptProperty.Convert(property)
							  ).ToList(),
			};
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
