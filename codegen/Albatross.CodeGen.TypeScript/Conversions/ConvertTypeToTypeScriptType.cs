﻿using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using System;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertTypeToTypeScriptType : IConvertObject<Type, TypeScriptType> {
		public TypeScriptType Convert(Type type) {
			if (type == typeof(string) || type == typeof(char)) {
				return TypeScriptType.String();
			} else if (type == typeof(float) || type == typeof(decimal) || type == typeof(double)
				  || type == typeof(byte) || type == typeof(sbyte)
				  || type == typeof(short) || type == typeof(int) || type == typeof(long)
				  || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong)) {
				return TypeScriptType.Number();
			} else if (type == typeof(bool)) {
				return TypeScriptType.Boolean();
			} else if (type == typeof(void)) {
				return TypeScriptType.Void();
			} else if (type == typeof(DateOnly) || type == typeof(DateTime)) {
				return TypeScriptType.Date();
			} else if (type == typeof(TimeSpan)) {
				return TypeScriptType.String();
			} else if (type == typeof(byte[])) {
				return TypeScriptType.String();
			} else if (type == typeof(object)) {
				return TypeScriptType.Any();
			} else if (type.GetCollectionElementType(out Type elementType)) {
				var result = this.Convert(elementType);
				result.IsArray = true;
				return result;
			} else if (type.GetNullableValueType(out Type targetType)) {
				return this.Convert(targetType);
			} else {
				return new TypeScriptType(type);
			}
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
