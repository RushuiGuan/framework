using System;

namespace Albatross.Messaging.Core {
	/// <summary>
	/// Use the attribute to replace the default command name, which is the full class name
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class CommandNameAttribute : Attribute {
		public CommandNameAttribute(string name) {
			Name = name;
		}
		public string Name { get; }
	}
}
