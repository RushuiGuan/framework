using Albatross.CodeGen.Python;
using Albatross.CodeGen.Python.Models;
using Albatross.CodeGen.WebClient;
using Albatross.CodeGen.WebClient.Python;
using Albatross.Config;
using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.Utility {
	[Verb("create-python-api")]
	public class CreatePythonApiOptions : BaseOption {
		[Option('d', "directory", Required = true, HelpText = "directory to save the generated code")]
		public string Directory { get; set; } = string.Empty;
	}
	public class CreatePythonApi : UtilityBase<CreatePythonApiOptions> {
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.AddPythonCodeGen();
			services.AddWebClientCodeGen();
		}
		public CreatePythonApi(CreatePythonApiOptions option) : base(option) { }
		public Task<int> RunUtility(ILogger logger) {
			var webApi = new WebApiClass("SustainalyticsApi", "/api/sustainalytics");
			var module = new PythonModule("api");
			module.Classes.Add(webApi);
			module.Build();
			module.Generate(System.Console.Out);
			using(var writer = new StreamWriter(Path.Combine(Options.Directory, "api.py"))) {
				module.Generate(writer);
			}
			return Task.FromResult(0);
		}
	}
}