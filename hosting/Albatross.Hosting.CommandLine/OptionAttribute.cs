using System;

namespace Albatross.Hosting.CommandLine {
	public class OptionAttribute : Attribute{
		public OptionAttribute(string name, string? description = null) {
			Name = name;
			Description = description;
		}

		public string Name { get; }
		public string? Description { get; }
		public string[] Alias { get; set; } = new string[0];
		public bool Required { get; set; }
		public bool Hidden { get; set; }
	}
}
