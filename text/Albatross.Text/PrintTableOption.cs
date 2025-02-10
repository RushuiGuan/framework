using System;

namespace Albatross.Text {
	[Obsolete("Use Albatross.Text.Table instead")]
	public record class PrintTableOption : PrintOption {
		public bool PrintHeader { get; set; } = true;
		public Func<string, string> GetColumnHeader { get; set; } = args => args;
	}
}