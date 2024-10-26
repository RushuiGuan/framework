
using Albatross.CodeGen.CSharp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.CSharp.Conversions {
	public static class ConversionExtensions {
		public static AccessModifier GetAccessModifier(this MethodInfo method) {
			AccessModifier accessModifier = AccessModifier.None;

			if (method.IsPublic) {
				accessModifier = accessModifier | AccessModifier.Public;
			}
			if (method.IsPrivate) {
				accessModifier = accessModifier | AccessModifier.Private;
			}
			if (method.IsAssembly) {
				accessModifier = accessModifier | AccessModifier.Internal;
			}
			if (method.IsFamily) {
				accessModifier = accessModifier | AccessModifier.Protected;
			}
			return accessModifier;
		}
	}
}