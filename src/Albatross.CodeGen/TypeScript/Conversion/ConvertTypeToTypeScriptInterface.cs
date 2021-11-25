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
			var model = new Interface(type.Name) {
				Properties = (from property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
							 select convertPropertyInfoToTypeScriptProperty.Convert(property)).ToList(),
			};
			if (type.IsGenericType) {
				model.IsGeneric = true;
				model.GenericTypes = type.GetGenericArguments().Select(args => args.Name).ToList();
			}
			return model;
		}

		object IConvertObject<Type>.Convert(Type from) {
			return this.Convert(from);
		}
	}
}
