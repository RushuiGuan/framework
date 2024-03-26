using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Albatross.Reflection {
	public static class AssemblyExtensions {
		/// <summary>
		/// Return all concrete classes that derive from base class T in an assembly
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public static IEnumerable<Type> GetConcreteClasses<T>(this Assembly assembly) {
			return assembly.GetTypes().Where(args => args.IsConcreteType() && typeof(T).IsAssignableFrom(args));
		}

		/// <summary>
		/// Return all Concrete classes in an assembly
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public static IEnumerable<Type> GetConcreteClasses(this Assembly assembly) {
			return assembly.GetTypes().Where(args => args.IsConcreteType());
		}

		/// <summary>
		/// TODO: this doesn't work if the assembl name doesn't match the default namespace name
		/// </summary>
		public static string GetEmbeddedFile(this Type type, string name, string folder = "Embedded") {
			var attribute = type.Assembly.GetCustomAttribute<DefaultNamespaceAttribute>();
			string resourceName = $"{attribute?.DefaultNamespace ?? type.Assembly.GetName().Name}.{folder}.{name}";
			using var stream = type.Assembly.GetManifestResourceStream(resourceName);
			if (stream == null) throw new ArgumentException($"Assembly resource {resourceName} doesn't exist");
			return new StreamReader(stream).ReadToEnd();
		}
	}
}
