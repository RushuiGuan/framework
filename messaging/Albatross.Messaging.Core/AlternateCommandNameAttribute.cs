using System;

namespace Albatross.Messaging.Core {
	/// <summary>
	/// Use this attribute to specify an alternate command name.  It is usefully to maintain backward compatibility when the class name of 
	/// the command changes but the consumers (clients) are still reference the old commands dll therefore using the old name.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class AlternateCommandNameAttribute : Attribute {
		public AlternateCommandNameAttribute(string name) {
			Name = name;
		}
		public string Name { get; }
	}
}
