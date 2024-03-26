using System;

namespace Albatross.Reflection {
	/// <summary>
	/// Use this attribute to specify the default namespace for embedded resources.  
	/// Useful when the assembly name doesn't match the default namespace name.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class DefaultNamespaceAttribute : Attribute {
		public string DefaultNamespace { get; }
		public DefaultNamespaceAttribute(string defaultNamespace) {
			this.DefaultNamespace = defaultNamespace;
		}
	}
}
