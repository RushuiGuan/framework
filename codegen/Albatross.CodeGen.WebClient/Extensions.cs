using Albatross.CodeGen.CSharp;
using Microsoft.Extensions.DependencyInjection;
using Albatross.CodeGen.TypeScript;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Albatross.CodeGen.WebClient.CSharp;
using Albatross.CodeGen.WebClient.TypeScript;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeGen.WebClient.TypeScriptOld;
using Albatross.CodeGen.WebClient.CSharpOld;
using Albatross.CodeGen.CommandLine;

namespace Albatross.CodeGen.WebClient {
	public static class Extensions {

		public static IServiceCollection AddWebClientCodeGen(this IServiceCollection services) {
			services.AddCSharpCodeGen().AddTypeScriptCodeGen();
			services.AddCodeGen(typeof(Extensions).Assembly);
			// symbol to model conversion
			services.AddScoped<ConvertClassSymbolToDtoClassModel>()
				.AddScoped<ConvertEnumSymbolToDtoEnumModel>()
				.AddScoped<ConvertApiControllerToControllerModel>();

			// model to code conversion
			services.AddScoped<ConvertWebApiToCSharpCodeStack>()
				.AddScoped<ConvertControllerModelToTypeScriptFile>()
				.AddScoped<CreateHttpClientRegistrations>()
				.AddScoped<ConvertDtoClassModelToTypeScriptInterface>()
				.AddScoped<ConvertEnumModelToTypeScriptEnum>();
			return services;
		}

		public static IServiceCollection AddLegacyWebClientCodeGen(this IServiceCollection services) {
			services.AddCSharpCodeGen().AddTypeScriptCodeGen();
			services.TryAddSingleton<ICreateApiCSharpProxy, CreateApiCSharpProxy>();
			services.TryAddSingleton<ICreateAngularPublicApi, CreateAngularPublicApi>();
			services.TryAddScoped<ICreateWebClientMethod, CreateWebClientMethod>();
			services.TryAddSingleton<ConvertApiControllerToCSharpClass>();
			services.TryAddScoped<ConvertApiControllerToTypeScriptFile>();
			return services;
		}
	}
}
