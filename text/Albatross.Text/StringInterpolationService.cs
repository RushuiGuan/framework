using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;

namespace Albatross.Text {
	public static class StringInterpolationExtensions {
		public static string Interpolate<T>(this string input, Func<string, T, string> func, T value, bool throwException = false) {
			return new StringInterpolationService(new Microsoft.Extensions.Logging.Abstractions.NullLogger<StringInterpolationService>())
				.Interpolate(input, func, value, throwException);
		}
		public static string Interpolate(this string input, Func<string, string> func) {
			return new StringInterpolationService(new Microsoft.Extensions.Logging.Abstractions.NullLogger<StringInterpolationService>())
				.Interpolate<object?>(input, (name, obj) => func(name), null, true);
		}
	}

	public interface IStringInterpolationService {
		string Interpolate<T>(string input, Func<string, T, string> func, T value, bool throwException = false);
	}

	public class StringInterpolationService : IStringInterpolationService {
		public StringInterpolationService(ILogger logger) {
			this.logger = logger;
		}
		/// <summary>
		/// the pattern looks for text within the "${" and "}" with no space allowed at its beginning or the end.
		/// </summary>
		public const string ExpressionSearchPattern = @"\$\{(?!\s)(.+?)(?<![\s])}";
		public static readonly Regex ExpressionSearchRegex = new Regex(ExpressionSearchPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
		private readonly ILogger logger;

		public string Interpolate<T>(string input, Func<string, T, string> func, T value, bool throwException = false) {
			return ExpressionSearchRegex.Replace(input, (match) => {
				string expression = match.Groups[1].Value;
				try {
					return func(expression, value);
				} catch (Exception err) {
					if (throwException) {
						throw new InvalidOperationException($"expression parsing exception: {expression}, {err.Message}");
					} else {
						logger.LogError(err, "expression parsing exception: {expression}", expression);
						return match.Groups[0].Value;
					}
				}
			});
		}
	}
}