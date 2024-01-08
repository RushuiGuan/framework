using Albatross.CodeGen.Python;
using Albatross.CodeGen.Python.Models;
using Albatross.CodeGen.WebClient;
using Albatross.Config;
using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReferenceData.Core.Sustainalytics;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.Utility {
	[Verb("create-python-dto")]
	public class CreatePythonDtoOptions : BaseOption {
		[Option('d', "directory", Required = true, HelpText = "directory to save the generated code")]
		public string Directory { get; set; } = string.Empty;

		[Option("data-class")]
		public bool DataClass { get; set; }
	}
	public class CreatePythonDto : UtilityBase<CreatePythonDtoOptions> {
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.AddPythonCodeGen();
			services.AddWebClientCodeGen();
		}
		public CreatePythonDto(CreatePythonDtoOptions option) : base(option) { }
		public Task<int> RunUtility(ILogger logger, ICreatePythonDto converter) {
			var module = converter.Generate([typeof(ReferenceData.Core.EsgScoreDto).Assembly], new System.Type[0], new PythonModule[0], Options.Directory, "dto", IsValidType, Options.DataClass);
			module.Generate(System.Console.Out);
			return Task.FromResult(0);
		}
		bool IsValidType(System.Type type) => type != typeof(ApiValueConverter) 
			&& type != typeof(String2IntConverter) 
			&& type != typeof(HistoryFileValueConverter);
	}
}