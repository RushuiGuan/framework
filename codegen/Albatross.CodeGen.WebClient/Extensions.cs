using Microsoft.Extensions.DependencyInjection;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeGen.WebClient.CSharp;
using Albatross.CodeGen.WebClient.TypeScript;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeGen.CommandLine;

namespace Albatross.CodeGen.WebClient {
	public static class Extensions {

		public static IServiceCollection AddWebClientCodeGen(this IServiceCollection services) {
			services.AddTypeScriptCodeGen();
			services.AddCodeGen(typeof(Extensions).Assembly);
			// symbol to model conversion
			services.AddScoped<ConvertClassSymbolToDtoClassModel>()
				.AddScoped<ConvertEnumSymbolToDtoEnumModel>()
				.AddScoped<ConvertApiControllerToControllerModel>();

			// model to code conversion
			services.AddScoped<ConvertWebApiToCSharpCodeStack_Client740>()
				.AddScoped<ConvertControllerModelToTypeScriptFile>()
				.AddScoped<CreateHttpClientRegistrations>()
				.AddScoped<ConvertDtoClassModelToTypeScriptInterface>()
				.AddScoped<ConvertEnumModelToTypeScriptEnum>();
			return services;
		}
	}
}
