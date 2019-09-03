using Albatross.Database;
using System.Management.Automation;

namespace Albatross.CodeGen.PowerShell {
	[Cmdlet(VerbsCommon.New, "DatabaseTable")]
	public class NewDatabaseTable : Cmdlet {

		[Parameter(Position = 0, Mandatory = true)]
		public string Name { get; set; }

		[Parameter(Position = 1, Mandatory = true)]
		public string Schema { get; set; }

		[Parameter(Position = 2, Mandatory = true, ValueFromPipeline = true)]
		public Albatross.Database.Database Database { get; set; }


		protected override void ProcessRecord() {
			var table = new Table {
				Name = Name,
				Schema = Schema,
				Database = Database,
			};
			WriteObject(table);
		}
	}
}