using Albatross.Authentication.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Authentication {
	public class GetCurrentWindowsUser : IGetCurrentUser {
		public string AuthenticationType => "Windows";

		public string Get() {
			string name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
			int i = name.IndexOf('\\');
			if (i >= 0) {
				name = name.Substring(i + 1);
			}
			return name;
		}
	}
}
