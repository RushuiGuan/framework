using System;

namespace ThirdParty {
	public class CommandInterfaceAttribute : Attribute { }
	[CommandInterface]
	public partial interface IMyCommand { }
}
