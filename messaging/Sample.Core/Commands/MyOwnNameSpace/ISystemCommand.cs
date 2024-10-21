using System;

namespace Sample.Core.Commands.MyOwnNameSpace {
	public class CommandInterfaceAttribute : Attribute { }
	[CommandInterface]
	public partial interface ISystemCommand{ 
		bool Callback { get; }
	}
}
