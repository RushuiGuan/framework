namespace Albatross.Authentication.Windows {
	public class GetCurrentWindowsUser : IGetCurrentUser {
		public string Provider => "Windows";

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