using System;
using System.Threading.Tasks;

namespace Albatross.Text {
	public record class PrintOption {
		public PrintOption(params string[] properties) {
			this.Properties = properties;
		}
		public delegate Task<string> FormatValueDelegate(object? entity, string property, object? value);
		public char ColumnHeaderLineCharacter { get; init; } = '-';
		public FormatValueDelegate FormatValue { get; init; } = DefaultFormatValue;
		public string[] Properties { get; init; }

		public static Task<string> DefaultFormatValue(object? _, string property, object? value) {
			switch (value) {
				case DateTime date:
					if (property.Contains("datetime", StringComparison.InvariantCultureIgnoreCase)) {
						return Task.FromResult($"{date:yyyy-MM-dd HH:mm:ssz}");
					} else {
						return Task.FromResult($"{date:yyyy-MM-dd}");
					}
				default:
					return Task.FromResult(Convert.ToString(value));
			}
		}
	}

	public record class PrintPropertiesOption : PrintOption {
		public Func<int, string?>? GetColumnHeader { get; init; }
		public Func<string, string> GetRowHeader { get; init; } = args => args;
		public bool HasColumnHeaderLine => GetColumnHeader != null;

		public PrintPropertiesOption(params string[] properties) : base(properties) { }
	}

	public record class PrintTableOption : PrintOption {
		public Func<string, string> GetColumnHeader { get; init; } = args => args;
		public PrintTableOption(params string[] properties) : base(properties) { }
	}
}
