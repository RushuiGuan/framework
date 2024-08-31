using System;

namespace Albatross.CommandLine {
	public class VerbAttribute : Attribute {
		public VerbAttribute(string name, string? description = null) {
			Name = name;
			Description = description;
		}

		public string Name { get; }
		public string? Description { get; }
		public string[] Alias { get; set; } = new string[0];
	}
}
