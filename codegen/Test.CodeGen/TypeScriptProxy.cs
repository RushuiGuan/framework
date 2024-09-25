using Albatross.CodeGen.TypeScript.Models;
using Albatross.CodeGen.WebClient;
using Albatross.CommandLine;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.Reflection;

namespace Test.CodeGen {
	[Verb("typescript-proxy", typeof(GenerateTypeScriptProxy))]
	public record TypeScriptProxyOptions {
		public string LibraryLocation { get; init; } = string.Empty;
		public string PublicApiLocation { get; init; } = string.Empty;
	}
	public class GenerateTypeScriptProxy : ICommandHandler {
		private readonly ICreateTypeScriptDto dtoSvc;
		private readonly ICreateApiTypeScriptProxy apiSvc;
		private readonly ICreateAngularPublicApi publicApiSvc;
		private TypeScriptProxyOptions options;

		public GenerateTypeScriptProxy(IOptions<TypeScriptProxyOptions> options, ICreateTypeScriptDto dtoSvc, ICreateApiTypeScriptProxy apiSvc, ICreateAngularPublicApi publicApiSvc) {
			this.options = options.Value;
			this.dtoSvc = dtoSvc;
			this.apiSvc = apiSvc;
			this.publicApiSvc = publicApiSvc;
		}

		public int Invoke(InvocationContext context) {
			throw new NotImplementedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			Albatross.IO.Extensions.EnsureDirectory(Path.Join(options.LibraryLocation, "test.txt"));
			Albatross.IO.Extensions.EnsureDirectory(Path.Join(options.PublicApiLocation, "test.txt"));
			var dtoFile = dtoSvc.Generate(new Assembly[] { typeof(Test.Dto.Classes.ArrayValueType).Assembly, }, [], [],
				options.LibraryLocation, "dto", args => true);

			var apiFiles = apiSvc.Generate("test", null, [typeof(Test.WebApi.MyStartup).Assembly,],
				new TypeScriptFile[] { dtoFile, },
				options.LibraryLocation, null, AddImport);

			var allFiles = new TypeScriptFile[] { dtoFile, }.Union(apiFiles).ToArray();
			publicApiSvc.Generate(options.PublicApiLocation, options.LibraryLocation, allFiles);
			return Task.FromResult(0);
		}
		void AddImport(Class @class) {
			@class.Imports.Add(new Import("shared-core", "ConfigService", "DataService", "Logger"));
		}
	}
}
