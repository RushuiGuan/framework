using System;

namespace Albatross.CommandLine {
	public class OptionAttribute : Attribute {
		public OptionAttribute(params string[] alias) {
			this.Alias = alias;
		}
		public string? Description { get; set; }
		public string[] Alias { get; }
		public bool Hidden { get; set; }
		public bool Required { get; set; }
		public bool Ignore { get; set; }
	}
	public class ArgumentAttribute : Attribute {
		public ArgumentAttribute(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public string? Description { get; set; }
		public bool Hidden { get; set; }
	}
}