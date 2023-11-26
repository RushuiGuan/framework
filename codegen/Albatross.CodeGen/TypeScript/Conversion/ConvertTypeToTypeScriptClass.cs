using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Conversion {
	public class ConvertTypeToTypeScriptClass : IConvertObject<Type, TypeScript.Model.Class> {
		ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty;
		public ConvertTypeToTypeScriptClass(ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty) {
			this.convertPropertyInfoToTypeScriptProperty = convertPropertyInfoToTypeScriptProperty;
		}
		public TypeScript.Model.Class Convert(Type type) {
			return new Class(type.Name) {
				Properties = (from property in type.GetProperties() 
							  select convertPropertyInfoToTypeScriptProperty.Convert(property)
							  ).ToList(),
			};
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
