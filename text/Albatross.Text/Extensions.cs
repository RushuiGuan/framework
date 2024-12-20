﻿using Microsoft.Extensions.DependencyInjection;
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
		public static string TrimDecimal(this decimal value) {
			var text = $"{value}";
			int lastDigitToTrim = text.Length;
			for (int i = text.Length - 1; i >= 0; i--) {
				var c = text[i];
				if ((c == '0' || c == '.') && lastDigitToTrim == i + 1) {
					lastDigitToTrim = i;
				}
				if (c == '.') {
					if (lastDigitToTrim != text.Length) {
						return text.Substring(0, lastDigitToTrim);
					} else {
						return text;
					}
				}
			}
			return text;
		}
	}
}