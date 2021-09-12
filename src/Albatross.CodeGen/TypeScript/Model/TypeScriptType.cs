using Albatross.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model {
	public class TypeScriptType {
		const string VoidType = "void";
		const string PromiseType = "Promise";

		public string Name { get; set; }
		public bool IsGeneric { get; }
		public bool IsArray { get; set; }
		public TypeScriptType[] GenericTypeArguments { get; }
		public bool IsAsync => this.Name == PromiseType;
		public bool IsVoid => Name == VoidType || IsAsync && !IsGeneric;

		public TypeScriptType() { }
		public TypeScriptType(string name) : this(name, false, false, null) { }
		public TypeScriptType(string name, bool isArray, bool isGeneric, TypeScriptType[] genericTypeArguments) {
			this.Name = name;
			this.IsArray = isArray;
			this.IsGeneric = isGeneric;
			this.GenericTypeArguments = genericTypeArguments?.ToArray() ?? new TypeScriptType[0];
		}
		public TypeScriptType(Type type) {
			IsArray = type.IsArray;
			if (IsArray) {
				type = type.GetElementType();
			}
			if(type.GetNullableValueType(out var valueType)) {
				type = valueType;
			}
			IsGeneric = type.IsGenericType;
			if (IsGeneric) {
				Name = ReflectionExtension.GetGenericTypeName(type.GetGenericTypeDefinition().Name);
				GenericTypeArguments = (from item in type.GetGenericArguments() select new TypeScriptType(item)).ToArray();
			} else {
				Name = type.FullName;
				GenericTypeArguments = new TypeScriptType[0];
			}
		}

		public override string ToString() {
			StringWriter writer = new StringWriter();
			new Writer.WriteTypeScriptType().Run(writer, this);
			return writer.ToString();
		}
		public override bool Equals(object obj) {
			if (obj is TypeScriptType) {
				TypeScriptType input = (TypeScriptType)obj;
				bool result = input.Name == Name && input.IsArray == IsArray
					&& input.IsGeneric == IsGeneric && input.IsVoid == IsVoid
					&& input.GenericTypeArguments?.Length == GenericTypeArguments?.Length;
				if (result) {
					for (int i = 0; i < GenericTypeArguments.Length; i++) {
						if (input.GenericTypeArguments[i]?.Equals(GenericTypeArguments[i]) != true) {
							return false;
						}
					}
				}
				return result;
			}
			return false;
		}
		public override int GetHashCode() {
			return Name?.GetHashCode() ?? 0;
		}

		public static TypeScriptType Date() => new TypeScriptType("Date");
		public static TypeScriptType String() => new TypeScriptType("string");
		public static TypeScriptType Boolean() => new TypeScriptType("boolean");
		public static TypeScriptType Number() => new TypeScriptType("number");
		public static TypeScriptType Any() => new TypeScriptType("any");
		public static TypeScriptType Void() => new TypeScriptType(VoidType);
		public static TypeScriptType MakeAsync(TypeScriptType typescriptType) {
			if (!typescriptType.IsAsync) {
				if (typescriptType.IsVoid) {
					return new TypeScriptType(PromiseType);
				} else {
					return new TypeScriptType(PromiseType, false, true, new TypeScriptType[] { typescriptType });
				}
			}
			return typescriptType;
		}
		public TypeScriptType RemoveAsync() {
			if (IsAsync) {
				if (IsVoid) {
					return Void();
				} else {
					return GenericTypeArguments.First();
				}
			} else {
				return this;
			}
		}
	}
}
