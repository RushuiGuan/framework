using System;
using System.Collections.Generic;

namespace Albatross.CodeGen {
	public interface IModuleCodeElement : ICodeElement, IEnumerable<IModuleCodeElement> {
		string Name { get; }
		string Module { get; }
		string Tag { get; set; }
		void Build();
	}
}
