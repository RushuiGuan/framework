using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;

namespace Albatross.Text {
	public static class StringInterpolationExtensions {
		public static string Interpolate(this string input, Func<string, string> func) {
			return Interpolate<object?>(input, (name, _) => func(name), null, true);
		}
		/// <summary>
		/// the pattern looks for text within the "${" and "}" with no space allowed at its beginning or the end.
		/// </summary>
		public const string ExpressionSearchPattern = @"\$\{(?!\s)(.+?)(?<![\s])}";
		public static readonly Regex ExpressionSearchRegex = new Regex(ExpressionSearchPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

		public static string Interpolate<T>(this string input, Func<string, T, string> func, T value, bool throwException = false, ILogger? logger = null) {
			return ExpressionSearchRegex.Replace(input, (match) => {
				string expression = match.Groups[1].Value;
				try {
					return func(expression, value);
				} catch (Exception err) {
					if (throwException) {
						throw new InvalidOperationException($"expression parsing exception: {expression}, {err.Message}");
					} else {
						logger?.LogError(err, "expression parsing exception: {expression}", expression);
						return match.Groups[0].Value;
					}
				}
			});
		}
	}
}