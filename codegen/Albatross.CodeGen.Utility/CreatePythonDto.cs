using Albatross.CodeGen.Python;
using Albatross.CodeGen.WebClient;
using Albatross.Config;
using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.Utility {
	[Verb("create-python-dto")]
	public class CreatePythonDtoOptions : BaseOption {
		[Option('d', "directory", Required = true, HelpText = "directory to save the generated code")]
		public string Directory { get; set; } = string.Empty;

		[Option("data-class")]
		public bool DataClass { get; set; }

		[Option('s', "solution", Required = true)]
		public string Solution{get;set;	} = string.Empty;

		[Option('p', "projects", Required = true)]
		public IEnumerable<string> Projects  { get; set; } = Array.Empty<string>();

		[Option("skip")]
		public IEnumerable<string> SkipClass { get; set; } = Array.Empty<string>();


	}
	public class CreatePythonDto : UtilityBase<CreatePythonDtoOptions> {
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.AddPythonCodeGen();
			services.AddWebClientCodeGen();
		}
		public CreatePythonDto(CreatePythonDtoOptions option) : base(option) { }
		public async Task<int> RunUtility(ILogger logger, ICreatePythonDto converter) {
			if (!Directory.Exists(Options.Directory)) { Directory.CreateDirectory(Options.Directory); }
			using var workspace = MSBuildWorkspace.Create();
			var solution = await workspace.OpenSolutionAsync(Options.Solution);
			foreach(var project in solution.Projects) {
				if (Options.Projects.Contains(project.Name, StringComparer.InvariantCultureIgnoreCase)) {
					var compilation = await project.GetCompilationAsync();
					if (compilation != null) {
						foreach (var @class in compilation.SyntaxTrees.SelectMany(x => x.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())) {
							logger.LogInformation($"found class {@class.Identifier.Text}");
						}
					} else {
						logger.LogError("cannot compile project {project}", project.Name);
						return 1;
					}
				}
			}
			//var module = converter.Generate(list, [], Array.Empty<PythonModule>(), Options.Directory, "dto", IsValidType, Options.DataClass);
			//var writer = new StringWriter();
			//module.Generate(writer);
			//Options.WriteOutput(writer.ToString());
			return 0;
		}
		bool IsValidType(System.Type type) => !Options.SkipClass.Contains(type.Name);
	}
}