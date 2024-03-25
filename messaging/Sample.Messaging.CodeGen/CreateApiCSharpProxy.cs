using Albatross.CodeGen;
using Albatross.CodeGen.WebClient;
using Albatross.Config;
using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Messaging.WebApi.Controllers;
using System.Reflection;
using System.Threading.Tasks;

namespace Sample.Messaging.CodeGen {
	[Verb("create-api-csharp-proxy", HelpText = "Generate Api CSharp Proxy")]
	public class CreateApiCSharpProxyOption {
		[Option('o', "out", Required = true, HelpText = "Set the output folder")]
		public string Out { get; set; } = null!;

		[Option('p', "pattern", Required = false, HelpText = "Regex pattern to select the target classes")]
		public string? Pattern { get; set; }

		[Option('n', "namespace", Required = true, HelpText = "Target namespace")]
		public string Namespace { get; set; } = null!;
	}

	public class CreateApiCSharpProxy : UtilityBase<CreateApiCSharpProxyOption> {
		public CreateApiCSharpProxy(CreateApiCSharpProxyOption option) : base(option) { }
		
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environmentSetting, IServiceCollection services) {
			base.RegisterServices(configuration, environmentSetting, services);
			services.AddDefaultCodeGen();
			services.AddWebClientCodeGen();
		}

		public Task<int> RunUtility(ICreateApiCSharpProxy svc) {
			Assembly[] asms = new Assembly[] {
				typeof(RunController).Assembly ,
			};
			svc.Generate(Options.Pattern, Options.Namespace, asms, Options.Out);
			return Task.FromResult(0);
		}
	}
}