using Albatross.Database;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace Albatross.CodeGen.PowerShell {
	[Cmdlet(VerbsCommon.Get, "DatabaseTable")]
	public class GetDatabaseTable : BaseCmdlet<IListTable> {
		[Parameter(Position = 0)]
		public string Criteria { get; set; }

		[Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true)]
		public Albatross.Database.Database Database { get; set; }

		protected override void ProcessRecord() {
			var items = EntryObject.Get(Database, Criteria);
			foreach(var item in items) {
				WriteObject(item);
			}
		}

		protected override void RegisterContainer(IServiceCollection svc) {
		}
	}
}
