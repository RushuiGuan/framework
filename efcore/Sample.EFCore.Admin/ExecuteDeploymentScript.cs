using Albatross.CommandLine;
using Albatross.EFCore.SqlServer;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.EFCore.Admin {
	[Verb("exec-script", typeof(ExecuteDeploymentScript), Description = "Execute deployment scripts")]
	public class ExecuteDeploymentScriptOption {
		[Option("l")]
		public string? Location { get; set; }
	}

	public class ExecuteDeploymentScript : BaseHandler<ExecuteDeploymentScriptOption> {
		private readonly SqlServerMigration<SampleSqlServerMigration> svc;

		public ExecuteDeploymentScript(SqlServerMigration<SampleSqlServerMigration> svc, IOptions<ExecuteDeploymentScriptOption> options) : base(options) {
			this.svc = svc;
		}

		public override async Task<int> InvokeAsync(InvocationContext context) {
			await svc.ExecuteDeploymentScript(options.Location ?? "Scripts");
			return 0;
		}
	}
}