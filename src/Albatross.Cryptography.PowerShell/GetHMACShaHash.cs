using Albatross.Cryptography.Core;
using System.Management.Automation;

namespace Albatross.Cryptography.PowerShell {
	[Cmdlet(VerbsCommon.Get, "HMACSHAHash")]
	public class GetHMACShaHash : PSCmdlet {

		[Parameter(Mandatory =true, Position =0)]
		public string Value{ get; set; }

		[Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true)]
		public string Salt{ get; set; }

		protected override void ProcessRecord() {
			WriteObject($"Hash: {Value.HMACSHAHash(Salt)}");
			WriteObject($"Salt: {Salt}");
		}
	}
}
