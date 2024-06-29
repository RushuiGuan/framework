﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen {
	public interface ICodeModule {
		string Name { get; }
		IModuleCodeElement Add(ICodeElement element);
	}
}
