using System.Linq;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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
		public static IServiceCollection AddWebClientCodeGen(this IServiceCollection services) {
			services.AddCodeGen(typeof(Extension).Assembly);
			services.AddSingleton<ICreateApiCSharpProxy, CreateApiCSharpProxy>();
			services.AddSingleton<ICreateApiTypeScriptProxy, CreateApiTypeScriptProxy>();
			services.AddSingleton<ICreateTypeScriptDto, CreateTypeScriptDto>();
			services.AddSingleton<IConvertDtoToTypeScriptInterface, ConvertDtoToTypeScriptInterface>();
			services.AddSingleton<ICreateAngularPublicApi, CreateAngularPublicApi>();
			return services;
		}
    }
}
