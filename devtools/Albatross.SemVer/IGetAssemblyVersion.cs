using System;
using System.Collections.Generic;

namespace Albatross.SemVer {
    public interface IGetAssemblyVersion {
		Version Get(string current, SematicVersion sematicVersion);
    }
}
