using Albatross.CodeGen.TypeScript.Conversions;
using Albatross.Reflection;
using Albatross.Text;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.CodeGen.TypeScript.Models {
	public class TypeScriptType : ICodeElement, IEquatable<TypeScriptType>{
		public const string VoidType = "void";
		public const string PromiseType = "Promise";

		public string Name { get; set; }
		public bool IsGeneric { get; set; }
		public bool IsArray { get; set; }
		public TypeScriptType[] GenericTypeArguments { get; set; } = new TypeScriptType[0];
		public bool IsAsync => this.Name == PromiseType;
		public bool IsVoid => Name == VoidType || IsAsync && !IsGeneric;
		public bool IsNullable { get; set; }


		public TypeScriptType(string name) : this(name, false, false, new TypeScriptType[0]) { }
		public TypeScriptType(string name, bool isArray, bool isGeneric, params TypeScriptType[] genericTypeArguments) {
			this.Name = name;
			this.IsArray = isArray;
			this.IsGeneric = isGeneric;
			this.GenericTypeArguments = genericTypeArguments?.ToArray() ?? new TypeScriptType[0];
		}

		public override string ToString() {
			StringWriter writer = new StringWriter();
			writer.Code(this);
			return writer.ToString();
		}
		public bool Equals(TypeScriptType? other) {
			if (other != null) {
				bool result = other.Name == Name && other.IsArray == IsArray
					&& other.IsGeneric == IsGeneric && other.IsVoid == IsVoid
					&& other.GenericTypeArguments.Length == GenericTypeArguments.Length;
				if (result) {
					for (int i = 0; i < GenericTypeArguments.Length; i++) {
						if (other.GenericTypeArguments[i]?.Equals(GenericTypeArguments[i]) != true) {
							return false;
						}
					}
				}
				return result;
			} else {
				return false;
			}
		}
		public override bool Equals(object? obj) {
			if (obj is TypeScriptType) {
				return this.Equals((TypeScriptType)obj);
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
		public static TypeScriptType Blob() => new TypeScriptType("Blob");
		public static TypeScriptType Void() => new TypeScriptType(VoidType);
		public static TypeScriptType MakeAsync(TypeScriptType typescriptType) {
			if (!typescriptType.IsAsync) {
				return new TypeScriptType(PromiseType, false, true, typescriptType);
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

		public TextWriter Generate(TextWriter writer) {
			if (IsVoid && !IsAsync) {
				writer.Append("void");
			} else {
				writer.Append(Name);
				if (IsGeneric) {
					if (GenericTypeArguments?.Count() > 0) {
						writer.OpenAngleBracket();
						bool first = true;
						foreach (var genericType in GenericTypeArguments) {
							if (!first) {
								writer.Comma().Space();
							} else {
								first = false;
							}
							writer.Code(genericType);
						}
						writer.CloseAngleBracket();
					} else {
						throw new CodeGenException("Missing Generic Arguments");
					}
				}
				if (IsArray) {
					writer.Append("[]");
				}
			}
			return writer;
		}
	}
}
