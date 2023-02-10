using System;

namespace Albatross.Text {
	public class PrintOption {
		public delegate string FormatValueDelegate(string property, object? value);
		public char RowHeaderSeperator { get; set; } = '-';
		public FormatValueDelegate FormatValue { get; set; } = DefaultFormatValue;
		public static string DefaultFormatValue(string property, object? value) {
			switch (value) {
				case DateTime date:
					if (property.Contains("datetime", StringComparison.InvariantCultureIgnoreCase)) {
						return $"{date:yyyy-MM-dd HH:mm:ssz}";
					} else {
						return $"{date:yyyy-MM-dd}";
					}
				default:
					return Convert.ToString(value);
			}
		}
	}

	public class PrintPropertiesOption : PrintOption {
		public Func<int, string?>? GetRowHeader { get; set; }
		public Func<string, string> GetColumnHeader { get; set; } = args => args;
		public bool HasRowHeaderSeperator => GetRowHeader != null;
	}

	public class PrintTableOption : PrintOption {
		public Func<string, string> GetRowHeader { get; set; } = args => args;
	}
}
