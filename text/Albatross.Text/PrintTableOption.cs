﻿using System;

namespace Albatross.Text {
	[Obsolete("Use Albatross.TextGrid instead")]
	public record class PrintTableOption : PrintOption {
		public bool PrintHeader { get; set; } = true;
		public Func<string, string> GetColumnHeader { get; set; } = args => args;
	}
}