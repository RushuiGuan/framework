﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.Core {
	public interface IGetAssemblyLocation {
		string Directory { get; }
		string CodeBase { get; }
	}
}
