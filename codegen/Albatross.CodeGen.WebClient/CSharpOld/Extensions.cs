using System.Linq;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace Albatross.CodeGen.WebClient.CSharpOld {
	public static class Extensions {
		public static string GetMethod(this HttpMethodAttribute attribute) {
			if (attribute is HttpPatchAttribute) {
				return "GetPatchMethod()";
			} else {
				string text = attribute.HttpMethods.First();
				return $"HttpMethod.{text.Substring(0, 1)}{text.Substring(1).ToLower()}";
			}
		}
	}
}
