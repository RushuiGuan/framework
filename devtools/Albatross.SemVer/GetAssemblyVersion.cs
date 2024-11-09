using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Albatross.SemVer {

	public class GetAssemblyVersion : IGetAssemblyVersion {
		public Version Get(string current, SematicVersion sematicVersion) {
			Version version = new Version(current);
			if(version.Major == sematicVersion.Major && version.Minor == sematicVersion.Minor && version.Build == sematicVersion.Patch) {
				version = new Version(version.Major, version.Minor, version.Build, version.Revision + 1);
			} else {
				version = new Version(sematicVersion.Major, sematicVersion.Minor, sematicVersion.Patch, 0);
			}
			return version;
		}
	}
}
