using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.Core {
	public interface IGetEntryAssemblyLocation {
		string Directory { get; }
		string CodeBase { get; }
	}
}
