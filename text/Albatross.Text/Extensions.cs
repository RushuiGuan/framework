using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.Text {
	public static class Extensions {
		public static IServiceCollection AddStringInterpolation(this IServiceCollection services) {
			services.TryAddSingleton<IStringInterpolationService, StringInterpolationService>();
			return services;
		}

		public static string TrimStart(this string line, string value) {
			if (line.StartsWith(value)) {
				return line.Substring(value.Length);
			} else {
				return line;
			}
		}
		public static string TrimEnd(this string line, string value) {
			if (line.EndsWith(value)) {
				return line.Substring(0, line.Length - value.Length);
			} else {
				return line;
			}
		}

		public static bool TryGetText(this string line, char delimiter, ref int offset, [NotNullWhen(true)] out string? text) {
			if (offset == line.Length + 1) {
				text = null;
				return false;
			} else {
				var index = line.IndexOf(delimiter, offset);
				if (index == -1) {
					text = line.Substring(offset);
					offset = line.Length + 1;
					return true;
				} else {
					text = line.Substring(offset, index - offset);
					offset = index + 1;
					return true;
				}
			}
		}

		/// <summary>
		/// write a decimal number and remove its trailing zeros after the decimal point
		/// </summary>
		[Obsolete("Use Decimal2CompactString instead")]
		public static string TrimDecimal(this decimal value) => TrimDecimalTrailingZeros(value.ToString());
		public static string Decimal2CompactString(this decimal value) => $"{value:G29}";
		/// <summary>
		/// this method is useful when a decimal number should be compacted after formatting.
		/// Here is an example to format 2000.5640000 as 2,000.564.
		/// var d = 2000.56400000M;
		/// var text = d.ToString("#,0.#############################").TrimDecimalTrailingZeros()
		/// </summary>
		/// <param name="decimalText"></param>
		/// <returns></returns>
		public static string TrimDecimalTrailingZeros(this string decimalText) {
			int lastDigitToTrim = decimalText.Length;
			for (int i = decimalText.Length - 1; i >= 0; i--) {
				var c = decimalText[i];
				if ((c == '0' || c == '.') && lastDigitToTrim == i + 1) {
					lastDigitToTrim = i;
				}
				if (c == '.') {
					if (lastDigitToTrim != decimalText.Length) {
						return decimalText.Substring(0, lastDigitToTrim);
					} else {
						return decimalText;
					}
				}
			}
			return decimalText;
		}
	}
}