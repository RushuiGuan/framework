using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Albatross.Reflection {
	public static partial class Extension {
		/// <summary>
		/// Return the generic argument of Nullable<>
		/// </summary>
		public static bool GetNullableValueType(this Type nullableType, [NotNullWhen(true)]out Type? valueType) {
			if (nullableType.IsGenericType && nullableType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
				valueType = nullableType.GetGenericArguments()[0];
				return true;
			}
			valueType = null;
			return false;
		}

		/// <summary>
		/// Return the generic argument of Task<>
		/// </summary>
		public static bool GetTaskResultType(this Type taskType, [NotNullWhen(true)] out Type? resultType) {
			if (taskType.IsGenericType && taskType.GetGenericTypeDefinition() == typeof(Task<>)) {
				resultType = taskType.GetGenericArguments()[0];
				return true;
			}
			resultType = null;
			return false;
		}

		/// <summary>
		/// Return the generic argument of IEnumerable<> or the element type of an array
		/// </summary>
		public static bool GetCollectionElementType(this Type collectionType, [NotNullWhen(true)] out Type? elementType) {
			elementType = null;

			if (collectionType == typeof(string)) {
				return false;
			} else if (collectionType == typeof(Array) || collectionType.IsArray) {
				elementType = collectionType.GetElementType()!;
				if (elementType == null) {
					elementType = typeof(object);
				}
			} else if (collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) {
				elementType = collectionType.GetGenericArguments().First();
			} else if (collectionType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))) {
				elementType = collectionType.GetGenericArguments().First();
			} else if (typeof(IEnumerable).IsAssignableFrom(collectionType)) {
				elementType = typeof(object);
			} else {
				return false;
			}
			return true;
		}

		public static string GetGenericTypeName(this string name) {
			return name.Substring(0, name.LastIndexOf('`'));
		}


		/// <summary>
		/// Check if a type is anoymous
		/// </summary>
		public static Boolean IsAnonymousType(this Type type) {
			Boolean hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
			Boolean nameContainsAnonymousType = type.FullName?.Contains("AnonymousType") == true;
			Boolean isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;
			return isAnonymousType;
		}

		public static bool IsConcreteType(this Type type) => !type.IsAbstract && !type.IsInterface && type.IsClass && !type.IsGenericTypeDefinition;

		/// <summary>
		/// return true if input parameter is derived from the generic type
		/// </summary>
		public static bool IsDerived<T>(this Type type) => typeof(T).IsAssignableFrom(type);
		public static bool IsDerived(this Type type, Type baseType) => baseType.IsAssignableFrom(type);


		/// <summary>
		/// Provide with a class type and a generic type\interface definition, this methods will return true if the class derives\implements the generic type\interface.  It will
		/// also output the generic type.
		/// 
		/// Example:
		/// public class Test: IEnumerable&lt;int&gt; { }
		/// 
		/// var result = typeof(Test).TryGetClosedGenericTypes(typeof(IEnumerable&lt;&gt;), out Type type);
		/// Assert.True(result);
		/// Assert.AreSame(typeof(IEnumerable&lt;int&gt;), type);
		/// </summary>
		/// <param name="type">Input class type</param>
		/// <param name="genericDefinition">The definition of a generic type.  For example: typeof(IEnumerable&lt;&gt;)</param>
		/// <param name="genericType">If the class extends\implements the generic type\interface, its type will be set in this output parameter</param>
		/// <returns>Return true if the class implements the generic interface</returns>
		public static bool TryGetClosedGenericType(this Type type, Type genericDefinition, [NotNullWhen(true)]out Type? genericType) {
			genericType = null;
			if (!type.IsAbstract && type.IsClass && !type.IsGenericTypeDefinition) {
				if (genericDefinition.IsInterface) {
					genericType = type.GetInterfaces().FirstOrDefault(args => args.IsGenericType && args.GetGenericTypeDefinition() == genericDefinition);
				} else {
					while (type != typeof(object)) {
						if (type.IsGenericType && type.GetGenericTypeDefinition() == genericDefinition) {
							genericType = type;
							break;
						}
						type = type.BaseType ?? typeof(object);
					}
				}
			}
			return genericType != null;
		}

		/// <summary>
		/// Type.GetType method returns null if class is not found.  This method will throw ArgumentException
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public static Type GetClass(this string? className) {
			if (string.IsNullOrEmpty(className)) {
				throw new ArgumentException("Type not found: empty class name");
			} else {
				Type? type = Type.GetType(className);
				if (type == null) {
					throw new ArgumentException($"Type not found: {className}");
				}
				return type;
			}
		}

		public static bool IsNullable(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		public static bool IsNumericType(this Type type) {
			switch (Type.GetTypeCode(type)) {
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Decimal:
				case TypeCode.Double:
				case TypeCode.Single:
					return true;
				default:
					return false;
			}
		}

		public static string GetTypeNameWithoutAssemblyVersion(this Type type) => $"{type.FullName}, {type.Assembly.GetName().Name}";

		public static A? GetEnumMemberAttribute<EnumType, A>(this EnumType enumValue) where A : Attribute where EnumType: notnull {
			var type = typeof(EnumType);
			if (type.IsEnum) {
				var value = enumValue.ToString();
				if (value != null) {
					var members = type.GetMember(value);
					return members[0].GetCustomAttribute<A>();
				} else {
					return null;
				}
			} else {
				throw new ArgumentException($"Type {type.FullName} is not an enum");
			}
		}

		public static string GetAssemblyLocation(this Assembly asm, string path) {
			string location = System.IO.Path.GetDirectoryName(asm.Location)??throw new Exception($"Cannot find the location of assembly {asm.FullName}");
			return System.IO.Path.Combine(location, path);
		}

		public static DirectoryInfo GetAssemblyDirectoryLocation(this Assembly asm, string path) {
			string location = GetAssemblyLocation(asm, path);
			return new DirectoryInfo(location);
		}

		public static FileInfo GetAssemblyFileLocation(this Assembly asm, string path) {
			string location = GetAssemblyLocation(asm, path);
			return new FileInfo(location);
		}

		/// <summary>
		/// return the property value of an object using reflection.  Property name can be delimited using . to allow retrieval of nested object property value
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		public static object? GetPropertyValue(this Type type, object? data, string name) {
			if (data == null) { return null; }
			var index = name.IndexOf('.');
			if(index == -1) {
				var property = type.GetProperty(name) ?? throw new ArgumentException($"Property {name} is not found in type {type.Name}");
				return property.GetValue(data);
			} else {
				var firstProperty = name.Substring(0, index);
				var property = type.GetProperty(firstProperty) ?? throw new ArgumentException($"Property {name} is not found in type {type.Name}");
				var value = property.GetValue(data);
				if(value != null) {
					var remainingProperty = name.Substring(index + 1);
					return GetPropertyValue(value.GetType(), value, remainingProperty);
				} else {
					return null;
				}
			}
		}
	}
}