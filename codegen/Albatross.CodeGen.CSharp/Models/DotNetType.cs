using Albatross.Reflection;
using Albatross.Text;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CSharp.Models {
	public class DotNetType : ICodeElement {
		public const string VoidType = "System.Void";
		public const string NullableValueType = "System.Nullable";

		public string Name { get; }
		public bool IsGeneric { get; }
		public bool IsArray { get; }
		public DotNetType[] GenericTypeArguments { get; }

		public bool IsAsync => this.Name == typeof(Task).FullName;
		public bool IsVoid => Name == VoidType || IsAsync && !IsGeneric;
		public bool IsValueType { get; set; } = false;
		public bool IsNullableReferenceType { get; set; } = false;
		public bool IsNullableValueType => this.Name == NullableValueType;

		public DotNetType(string name) : this(name, false, false, new DotNetType[0]) { }
		public DotNetType(string name, bool isArray, bool isGeneric, DotNetType[]? genericTypeArguments) {
			this.Name = name;
			this.IsArray = isArray;
			this.IsGeneric = isGeneric;
			this.GenericTypeArguments = genericTypeArguments?.ToArray() ?? new DotNetType[0];
		}
		public DotNetType(Type type, ParameterInfo parameterInfo) : this(type) {
			if (IsAsync && IsGeneric) {
				var subType = this.GenericTypeArguments.First();
				if (!subType.IsValueType) {
					subType.IsNullableReferenceType = new NullabilityInfoContext().Create(parameterInfo).GenericTypeArguments.First().WriteState is NullabilityState.Nullable;
				}
			} else if (!type.IsValueType) {
				IsNullableReferenceType = new NullabilityInfoContext().Create(parameterInfo).WriteState is NullabilityState.Nullable;
			}
		}
		public DotNetType(Type type) {
			IsArray = type.IsArray;
			if (IsArray) {
				type = type.GetElementType() ?? throw new InvalidOperationException($"Array type {type.Name} is missing its element type");
			}
			IsValueType = type.IsValueType;
			IsGeneric = type.IsGenericType;
			if (IsGeneric) {
				Name = type.GetGenericTypeDefinition().FullName?.GetGenericTypeName() ?? throw new Exception("impossible");
				GenericTypeArguments = (from item in type.GetGenericArguments() select new DotNetType(item)).ToArray();
			} else {
				Name = type.FullName ?? throw new Exception($"Type {type.Name} is missing its full name (maybe it is an anonymous type?)");
				GenericTypeArguments = new DotNetType[0];
			}
		}
		public DotNetType(DotNetType original) {
			this.Name = original.Name;
			this.IsGeneric = original.IsGeneric;
			this.IsArray = original.IsArray;
			GenericTypeArguments = original.GenericTypeArguments?.Select(args => new DotNetType(args)).ToArray() ?? new DotNetType[0];
			IsValueType = original.IsValueType;
			IsNullableReferenceType = original.IsNullableReferenceType;
		}

		public override string? ToString() {
			StringWriter writer = new StringWriter();
			return writer.Code(this).ToString();
		}
		public override bool Equals(object? obj) {
			if (obj != null && obj is DotNetType) {
				DotNetType input = (DotNetType)obj;
				bool result = input.Name == Name && input.IsArray == IsArray
					&& input.IsGeneric == IsGeneric && input.IsVoid == IsVoid
					&& input.GenericTypeArguments.Length == GenericTypeArguments.Length;
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

		public static DotNetType Void() => new DotNetType(VoidType);

		public static DotNetType String() => new DotNetType(typeof(string));
		public static DotNetType Char() => new DotNetType(typeof(char));

		public static DotNetType Short() => new DotNetType(typeof(short));
		public static DotNetType Integer() => new DotNetType(typeof(int));
		public static DotNetType Long() => new DotNetType(typeof(long));
		public static DotNetType Decimal() => new DotNetType(typeof(decimal));
		public static DotNetType Single() => new DotNetType(typeof(Single));
		public static DotNetType Double() => new DotNetType(typeof(double));

		public static DotNetType Object() => new DotNetType(typeof(object));

		public static DotNetType DateTime() => new DotNetType(typeof(DateTime));
		public static DotNetType DateOnly() => new DotNetType(typeof(DateOnly));
		public static DotNetType DateTimeOffset() => new DotNetType(typeof(DateTimeOffset));
		public static DotNetType TimeSpan() => new DotNetType(typeof(TimeSpan));

		public static DotNetType Boolean() => new DotNetType(typeof(bool));
		public static DotNetType Byte() => new DotNetType(typeof(byte));
		public static DotNetType ByteArray() => new DotNetType(typeof(byte[]));
		public static DotNetType Guid() => new DotNetType(typeof(Guid));

		public static DotNetType IDbConnection() => new DotNetType("System.Data.IDbConnection");

		public static DotNetType MakeNullable(DotNetType dotNetType) {
			if (dotNetType.IsValueType) {
				return new DotNetType(NullableValueType, false, true, new DotNetType[] { dotNetType });
			} else {
				return new DotNetType(dotNetType) {
					IsNullableReferenceType = true,
				};
			}
		}
		public static DotNetType MakeIEnumerable(DotNetType dotNetType) {
			return new DotNetType("System.Collections.Generic.IEnumerable", false, true, new DotNetType[] { dotNetType });
		}
		public static DotNetType MakeAsync(DotNetType dotNetType) {
			if (dotNetType.IsVoid) {
				return new DotNetType(typeof(Task).FullName ?? throw new Exception());
			} else {
				return new DotNetType(typeof(Task).FullName ?? throw new Exception(), false, true, new DotNetType[] { dotNetType });
			}
		}
		public DotNetType RemoveAsync() {
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
		public DotNetType RemoveNullable() {
			if (IsNullableReferenceType) {
				return new DotNetType(this) { IsNullableReferenceType = false };
			} else if (IsNullableValueType) {
				return this.GenericTypeArguments.First();
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
				if (IsNullableReferenceType) {
					writer.Append("?");
				}
			}
			return writer;
		}
	}
}