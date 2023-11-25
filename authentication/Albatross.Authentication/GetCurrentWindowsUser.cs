using Albatross.Authentication.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Authentication {
	public class GetCurrentWindowsUser : IGetCurrentUser {
		public string Provider => "Windows";

		public string Get() {
#pragma warning disable CA1416 // Validate platform compatibility
			string name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
#pragma warning restore CA1416 // Validate platform compatibility
			int i = name.IndexOf('\\');
			if (i >= 0) {
				name = name.Substring(i + 1);
			}
			return name;
		}
	}
}
