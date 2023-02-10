using System;

namespace Albatross.Text {
	public record class PrintOption {
		public PrintOption(params string[] properties) {
			this.Properties = properties;
		}
		public delegate string FormatValueDelegate(string property, object? value);
		public char RowHeaderSeperator { get; init; } = '-';
		public FormatValueDelegate FormatValue { get; init; } = DefaultFormatValue;
		public string[] Properties { get; init; }

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

	public record class PrintPropertiesOption : PrintOption {
		public Func<int, string?>? GetRowHeader { get; init; }
		public Func<string, string> GetColumnHeader { get; init; } = args => args;
		public bool HasRowHeaderSeperator => GetRowHeader != null;

		public PrintPropertiesOption(params string[] properties) : base(properties) { }
	}

	public record class PrintTableOption : PrintOption {
		public Func<string, string> GetRowHeader { get; init; } = args => args;
		public PrintTableOption(params string[] properties) : base(properties) { }
	}
}
