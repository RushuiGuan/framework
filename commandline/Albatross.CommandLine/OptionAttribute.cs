using System;

namespace Albatross.CommandLine {
	public class OptionAttribute : Attribute{
		public string? Description { get; set; }
		public string[] Alias { get; set; } = new string[0];
		public bool Hidden { get; set; }
		public bool Required { get; set; }
		public bool Skip { get; set; }
	}
}
