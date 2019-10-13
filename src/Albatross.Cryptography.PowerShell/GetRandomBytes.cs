using Albatross.Host.PowerShell;
using System;
using System.Management.Automation;

namespace Albatross.Cryptography.PowerShell {
	[Cmdlet(VerbsCommon.Get, "RandomBytes")]
	public class GetRandomBytes: PSCmdlet {
		[Parameter(Mandatory =false, Position = 0)]
		public int Size { get; set; } = 32;

		protected override void ProcessRecord() {
			byte[] bytes;
			using (var rng = new CryptoRNG()) {
				bytes = rng.GetBytes(Size);
			}
			WriteObject(Convert.ToBase64String(bytes));
		}
	}
}
