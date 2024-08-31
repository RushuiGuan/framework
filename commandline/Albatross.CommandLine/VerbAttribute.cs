using System;

namespace Albatross.CommandLine {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class VerbAttribute : Attribute {
		public VerbAttribute(string name, Type handler, string? description = null) {
			Name = name;
			Description = description;
			this.Handler = handler;
		}
		public Type Handler { get; }
		public string Name { get; }
		public string? Description { get; }
		public string[] Alias { get; set; } = new string[0];
	}
}
