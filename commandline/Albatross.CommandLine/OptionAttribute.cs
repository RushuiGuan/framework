using System;

namespace Albatross.CommandLine {
	public class OptionAttribute : Attribute{
		public OptionAttribute(params string[] alias) {
			this.Alias = alias;
		}
		public string? Description { get; set; }
		public string[] Alias { get; set; }
		public bool Hidden { get; set; }
		public bool Required { get; set; }
		public bool Skip { get; set; }
	}
}
