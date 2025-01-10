using Albatross.CommandLine;
using Albatross.EFCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.EFCore.Models;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sample.EFCore.Admin {
	[Verb("create-sql-script", typeof(GenerateSqlScript), Description = "Generate sql script for database")]
	public class GenerateSqlScriptOption {
		[Option("o", "output-file", Description = "Set the output file")]
		public string? Out { get; set; }

		[Option("d", "drop-script", Description = "Drop table scripts")]
		public string? DropScript { get; set; }
	}

	public class GenerateSqlScript : BaseHandler<GenerateSqlScriptOption> {
		private readonly SampleDbSession dbSession;

		public GenerateSqlScript(SampleDbSession dbSession, IOptions<GenerateSqlScriptOption> options) : base(options) {
			this.dbSession = dbSession;
		}
		public override Task<int> InvokeAsync(InvocationContext context) {
			string script = dbSession.GetCreateScript();
			using (StringReader reader = new StringReader(script)) {
				string content = reader.ReadToEnd();
				this.writer.WriteLine(content);
				if (!string.IsNullOrEmpty(options.Out)) {
					using (var file = new StreamWriter(options.Out)) {
						file.WriteLine(content);
					}
				}
			}
			if (!string.IsNullOrEmpty(options.DropScript)) {
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

				using (var writer = new StreamWriter(options.DropScript)) {
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