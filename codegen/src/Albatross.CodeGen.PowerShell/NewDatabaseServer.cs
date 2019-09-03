using System;
using System.Management.Automation;

namespace Albatross.CodeGen.PowerShell {
	[Cmdlet(VerbsCommon.New, "DatabaseServer")]
	public class NewDatabaseServer : PSCmdlet {

		const string PSName_ByComponent = "ByComponent";
		const string PSName_ByConnectionString = "ByConnectionString";

		[Alias("s")]
		[Parameter(ParameterSetName = PSName_ByComponent, Mandatory = true)]
		public string DataSource { get; set; }

		[Alias("d")]
		[Parameter(ParameterSetName = PSName_ByComponent)]
		public string InitialCatalog { get; set; }

		[Alias("i")]
		[Parameter(ParameterSetName = PSName_ByComponent)]
		public SwitchParameter IntegratedSecurity { get; set; }

		[Alias("u")]
		[Parameter(ParameterSetName = PSName_ByComponent)]
		public string User { get; set; }

		[Alias("p")]
		[Parameter(ParameterSetName = PSName_ByComponent)]
		public string Password { get; set; }

		[Alias("c")]
		[Parameter(ParameterSetName = PSName_ByConnectionString, Mandatory = true)]
		public string ConnectionString { get; set; }



		protected override void ProcessRecord() {
			if (base.ParameterSetName == PSName_ByComponent && !IntegratedSecurity.ToBool()) {
				if (string.IsNullOrEmpty(User)) { throw new ArgumentException("Parameter User is required"); }
				if (string.IsNullOrEmpty(Password)) { throw new ArgumentException("Parameter Password is required"); }
			}

			if (base.ParameterSetName == PSName_ByComponent) {
				WriteObject(
					new Albatross.Database.Database{
						DataSource = DataSource,
						InitialCatalog = InitialCatalog,
						SSPI = IntegratedSecurity.ToBool(),
						UserName = User,
						Password = Password,
					}
				);
			} else {
				WriteObject(new Albatross.Database.Database { ConnectionString = ConnectionString, });
			}
		}
	}
}