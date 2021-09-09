using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Conversion {
	public class ConvertTypeToTypeScriptInterface : IConvertObject<Type, TypeScript.Model.Interface> {
		ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty;
		public ConvertTypeToTypeScriptInterface(ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty) {
			this.convertPropertyInfoToTypeScriptProperty = convertPropertyInfoToTypeScriptProperty;
		}
		public TypeScript.Model.Interface Convert(Type type) {
			return new Interface {
				Name = type.Name,
				Export = true,
				Properties = from property in type.GetProperties() select convertPropertyInfoToTypeScriptProperty.Convert(property),
			};
		}

		object IConvertObject<Type>.Convert(Type from) {
			return this.Convert(from);
		}
	}
}
