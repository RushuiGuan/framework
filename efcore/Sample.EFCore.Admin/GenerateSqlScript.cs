using Albatross.EFCore;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Albatross.Config;
using Albatross.Hosting.Utility;
using System.Text.RegularExpressions;
using Sample.EFCore.Models;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace Sample.EFCore.Admin {
	[Verb("create-sql-script", HelpText = "Generate sql script for database")]
	public class GenerateSqlScriptOption : BaseOption {
		[Option('o', "output-file", Required = false, HelpText = "Set the output file")]
		public string? Out { get; set; }

		[Option('d', "drop-script", Required = false, HelpText = "Drop table scripts")]
		public string? DropScript { get; set; }
	}

	public class GenerateSqlScript : MyUtilityBase<GenerateSqlScriptOption> {
		public GenerateSqlScript(GenerateSqlScriptOption option) : base(option) {
		}

		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environmentSetting, IServiceCollection services) {
			base.RegisterServices(configuration, environmentSetting, services);
		}

		public Task<int> RunUtility(SampleDbSession dbSession) {
			string script = dbSession.GetCreateScript();
			using (StringReader reader = new StringReader(script)) {
				string content = reader.ReadToEnd();
				this.Options.WriteOutput(content);
				if (!string.IsNullOrEmpty(Options.Out)) {
					using (var file = new StreamWriter(Options.Out)) {
						file.WriteLine(content);
					}
				}
			}
			if (!string.IsNullOrEmpty(Options.DropScript)) {
				List<string> tables = new List<string>();
				Regex regex = new Regex(@"^CREATE TABLE (\[\w+\]\.\[\w+\]) \($", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				using (StringReader reader = new StringReader(script)) {
					for (string? line = reader.ReadLine(); line != null; line = reader.ReadLine()) {
						if (!string.IsNullOrEmpty(line)) {
							var match = regex.Match(line);
							if (match.Success) {
								tables.Add(match.Groups[1].Value);
							}
						}
					}
				}

				using (var writer = new StreamWriter(Options.DropScript)) {
					tables.Reverse();
					foreach (var table in tables) {
						if (table.StartsWith($"[{My.Schema.Sample}]")) {
							writer.Write("drop table ");
							writer.WriteLine(table);
						}
					}
					writer.WriteLine($"drop table [{My.Schema.Sample}].[__EFMigrationsHistory]");
				}
			}
			return Task.FromResult(0);
		}
	}
}
