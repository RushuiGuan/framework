using Albatross.CodeGen.Python;
using Albatross.CodeGen.Python.Models;
using Albatross.CodeGen.Tests.Dto;
using Albatross.CodeGen.WebClient;
using Albatross.Config;
using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.Utility {
	[Verb("python-dto")]
	public class PythonOptions :BaseOption{
	}
	public class Python : UtilityBase<PythonOptions> {
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.AddPythonCodeGen();
			services.AddWebClientCodeGen();
		}
		public Python(PythonOptions option) : base(option) {
		}
		public Task<int> RunUtility(ILogger logger, IConvertDtoToPythonClass converter) {
			var module = new Module("test");
			converter.ConvertClass(typeof(MyDto), module);
			module.Generate(System.Console.Out);
			return Task.FromResult(0);
		}
	}
}
