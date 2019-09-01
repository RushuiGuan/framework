using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Albatross.Reflection {
	public static partial class Extension {
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
	}
}
