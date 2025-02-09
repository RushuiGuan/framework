using System;
using System.Threading.Tasks;

namespace Albatross.Text {
	[Obsolete("Use Albatross.TextGrid instead")]
	public record class PrintOption {
		public delegate Task<string> FormatValueDelegate(object? entity, string property, object? value);
		public char ColumnHeaderLineCharacter { get; set; } = '-';
		public FormatValueDelegate FormatValue { get; set; } = DefaultFormatValue;
		public string[] Properties { get; set; } = Array.Empty<string>();

		public static Task<string> DefaultFormatValue(object? _, string property, object? value) {
			if (value is DateTime dateTime) {
				if (dateTime.TimeOfDay == TimeSpan.Zero) {
					return Task.FromResult($"{value:yyyy-MM-dd}");
				} else {
					return Task.FromResult($"{value:yyyy-MM-dd HH:mm:ssz}");
				}
			} else {
				return Task.FromResult(Convert.ToString(value) ?? string.Empty);
			}
		}
	}
}