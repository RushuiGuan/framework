using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Albatross.CodeGen {
	public static class ReflectionExtension {
		public static bool GetNullableValueType(this Type nullableType, out Type valueType) {
			valueType = null;
			if (nullableType.IsGenericType && nullableType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
				valueType = nullableType.GetGenericArguments()[0];
				return true;
			}
			return false;
		}

		public static bool GetCollectionElementType(this Type collectionType, out Type elementType) {
			elementType = null;

			if (collectionType == typeof(string)) {
				return false;
			} else if (collectionType == typeof(Array) || collectionType.IsArray) {
				elementType = collectionType.GetElementType();
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

		public static Boolean IsAnonymousType(this Type type) {
			Boolean hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
			Boolean nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
			Boolean isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;
			return isAnonymousType;
		}

		public static string GetGenericTypeName(string name) {
			return name.Substring(0, name.LastIndexOf('`'));
		}

		public static bool TryGetGenericInterfaceType(this Type type, Type genericInterfaceDefinition, out Type genericInterfaceType) {
			if (!type.IsAbstract && type.IsClass) {
				genericInterfaceType = type.GetInterfaces().FirstOrDefault(args => args.IsGenericType && args.GetGenericTypeDefinition() == genericInterfaceDefinition);
			} else {
				genericInterfaceType = null;
			}
			return genericInterfaceType != null;
		}
	}
}
