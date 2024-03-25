using Albatross.CodeGen.CSharp;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Albatross.CodeGen.TypeScript;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Albatross.CodeGen.WebClient {
	public static class Extensions {
		public static string GetMethod(this HttpMethodAttribute attribute) {
			if (attribute is HttpPatchAttribute) {
				return "GetPatchMethod()";
			} else {
				string text = attribute.HttpMethods.First();
				return $"HttpMethod.{text.Substring(0, 1)}{text.Substring(1).ToLower()}";
			}
		}
		public static IServiceCollection AddWebClientCodeGen(this IServiceCollection services) {
			services.AddCSharpCodeGen().AddTypeScriptCodeGen();
			services.TryAddSingleton<ICreateApiCSharpProxy, CreateApiCSharpProxy>();
			services.TryAddSingleton<ICreateApiTypeScriptProxy, CreateApiTypeScriptProxy>();
			services.TryAddSingleton<ICreateTypeScriptDto, CreateTypeScriptDto>();
			services.TryAddSingleton<IConvertDtoToTypeScriptInterface, ConvertDtoToTypeScriptInterface>();
			services.TryAddSingleton<ICreateAngularPublicApi, CreateAngularPublicApi>();
			services.TryAddSingleton<ConvertApiControllerToCSharpClass>();
			services.TryAddSingleton<ConvertApiControllerToTypeScriptClass>();
			return services;
		}
    }
}
