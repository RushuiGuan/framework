﻿using Albatross.CodeGen.TypeScript.Models;
using System;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertTypeToTypeScriptClass : IConvertObject<Type, TypeScript.Models.ClassDeclaration> {
		ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty;
		public ConvertTypeToTypeScriptClass(ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty) {
			this.convertPropertyInfoToTypeScriptProperty = convertPropertyInfoToTypeScriptProperty;
		}
		public TypeScript.Models.ClassDeclaration Convert(Type type) {
			return new ClassDeclaration(type.Name) {
				Properties = (from property in type.GetProperties() 
							  select convertPropertyInfoToTypeScriptProperty.Convert(property)
							  ).ToList(),
			};
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
