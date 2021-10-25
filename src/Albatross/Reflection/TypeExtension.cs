using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Albatross.Reflection {
	public static partial class Extension {
		/// <summary>
		/// Return the generic argument of Nullable<>
		/// </summary>
		public static bool GetNullableValueType(this Type nullableType, out Type valueType) {
			valueType = typeof(object);
			if (nullableType.IsGenericType && nullableType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
				valueType = nullableType.GetGenericArguments()[0];
				return true;
			}
			return false;
		}

		/// <summary>
		/// Return the generic argument of IEnumerable<> or the element type of an array
		/// </summary>
		public static bool GetCollectionElementType(this Type collectionType, out Type elementType) {
			elementType = typeof(object);

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
		public static bool IsDerived<T>(this Type type) => typeof(T).IsAssignableFrom(type);


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
		public static bool TryGetClosedGenericType(this Type type, Type genericDefinition, out Type genericType) {
			Type? result = null;
			if (!type.IsAbstract && type.IsClass && !type.IsGenericTypeDefinition) {
				if (genericDefinition.IsInterface) {
					result = type.GetInterfaces().FirstOrDefault(args => args.IsGenericType && args.GetGenericTypeDefinition() == genericDefinition);
				} else {
					while (type != typeof(object)) {
						if (type.IsGenericType && type.GetGenericTypeDefinition() == genericDefinition) {
							result = type;
							break;
						}
						type = type.BaseType ?? typeof(object);
					}
				}
			}
			genericType = result ?? typeof(object);
			return result != null;
		}

		/// <summary>
		/// Type.GetType method returns null if class is not found.  This method will throw ArgumentException
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public static Type GetClass(this string className) {
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

		public static DirectoryInfo GetAssemblyLocation(this Assembly asm, string subfolder) {
			string location = System.IO.Path.GetDirectoryName(asm.Location)??throw new Exception($"Cannot find the location of assembly {asm.FullName}");
			return new DirectoryInfo(System.IO.Path.Combine(location, subfolder));
		}
	}
}