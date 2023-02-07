using System;

namespace Albatross.Text {
	public class PrintPropertiesOption {
		public delegate string FormatValueDelegate(string property, object? value);
		public Func<int, string?>? GetHeader { get; set; }
		public FormatValueDelegate? FormatValue { get; set; }
		public char HeaderSeperator { get; set; } = '-';
		public bool HasHeaderSeperator => GetHeader != null;
		public PrintPropertiesOption() {
			FormatValue = (property, value) => {
				switch (value) {
					case DateTime date:
						if (property.Contains("datetime", StringComparison.InvariantCultureIgnoreCase)) {
							return $"{date:yyyy-MM-dd HH:mm:ss:z}";
						} else {
							return $"{date:yyyy-MM-dd}";
						}
					default:
						return Convert.ToString(value);
				}
			};
		}
	}
}
