﻿using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Conversion {
	public class ConvertTypeToTypeScriptInterface : IConvertObject<Type, TypeScript.Model.Interface> {
		ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty;
		private readonly ConvertTypeToTypeScriptType convertTypeToTypeScriptType;

		public ConvertTypeToTypeScriptInterface(ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty, ConvertTypeToTypeScriptType convertTypeToTypeScriptType) {
			this.convertPropertyInfoToTypeScriptProperty = convertPropertyInfoToTypeScriptProperty;
			this.convertTypeToTypeScriptType = convertTypeToTypeScriptType;
		}
		public TypeScript.Model.Interface Convert(Type type) {
			var model = new Interface(type.Name, type.IsGenericType, type.GetGenericArguments().Select(args => args.Name)) {
				Properties = (from property in type.GetProperties(System.Reflection.BindingFlags.Public
							  | System.Reflection.BindingFlags.DeclaredOnly
							  | System.Reflection.BindingFlags.Instance)
							  select convertPropertyInfoToTypeScriptProperty.Convert(property)).ToList(),
			};
			if (type.BaseType != null && type.BaseType != typeof(object) && type.BaseType != typeof(System.ValueType)) {
				model.BaseType = convertTypeToTypeScriptType.Convert(type.BaseType);
			}
			return model;
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
