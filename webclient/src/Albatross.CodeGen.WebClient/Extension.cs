using System.Linq;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using Albatross.CodeGen.Core;
using Microsoft.AspNetCore.Mvc;

namespace Albatross.CodeGen.WebClient {
    public static class Extension {
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
