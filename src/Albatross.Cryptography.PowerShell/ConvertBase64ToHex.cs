using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace Albatross.Cryptography.PowerShell {
	[Cmdlet(VerbsData.Convert, "Base64ToHex")]
	public class ConvertBase64ToHex : PSCmdlet {
		[Parameter(Mandatory =true, Position = 0, ValueFromPipeline =true)]
		public string Value { get; set; }

		protected override void ProcessRecord() {
			byte[] array = Convert.FromBase64String(Value);
			StringBuilder sb = new StringBuilder();
			sb.Append("0x");
			foreach(var item in array) {
				sb.AppendFormat("{0:x2}", item);
			}
			WriteObject(sb.ToString());
		}
	}
}
