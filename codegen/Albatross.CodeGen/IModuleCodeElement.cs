using System;
using System.Collections.Generic;

namespace Albatross.CodeGen {
	/// <summary>
	/// a module code element is a code element that can contain other code elements.  The code element also
	/// belongs to a module and has a name.  The module and name together can be used to reference the code element
	/// in the code file.
	/// 
	/// Here is an example in Python:
	/// from datetime import date
	/// 
	/// here the module is datetime and the name is date.
	/// 
	/// Here is an example in TypeScript:
	/// import { ConfirmationService, MenuItem } from 'primeng/api';
	/// 
	/// here the module is primeng/api and the name is ConfirmationService and MenuItem.
	/// 
	/// The interface is often useful for scripting langulages but not needed for compiled languages because references
	/// of compiled languages such as C# are tracked by the compiler and managed by the project file.
	/// 
	/// </summary>
	public interface IModuleCodeElement : ICodeElement {
		string Name { get; }
		string Module { get; }
		string Tag { get; set; }
		void Build();
	}
}
