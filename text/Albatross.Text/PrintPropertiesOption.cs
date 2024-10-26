using System;

namespace Albatross.Text {
	public record class PrintPropertiesOption : PrintOption {
		public Func<int, string?>? GetColumnHeader { get; set; }
		public Func<string, string> GetRowHeader { get; set; } = args => args;
		public bool HasColumnHeaderLine => GetColumnHeader != null;
	}
}