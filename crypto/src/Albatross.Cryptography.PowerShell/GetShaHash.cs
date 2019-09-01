using Albatross.Cryptography.Core;
using Albatross.Host.PowerShell;
using System;
using System.Management.Automation;

namespace Albatross.Cryptography.PowerShell {
	[Cmdlet(VerbsCommon.Get, "SHAHash")]
	public class GetShaHash : PSCmdlet {

		[Parameter(Mandatory =true, Position =0, ValueFromPipeline =true)]
		public string Value{ get; set; }

		[Parameter]
		public SwitchParameter SHA512 { get; set; }


		protected override void ProcessRecord() {
			if (SHA512.ToBool()) {
				WriteObject(Value.SHA512());
			} else {
				WriteObject(Value.SHA256());
			}
		}
	}
}
