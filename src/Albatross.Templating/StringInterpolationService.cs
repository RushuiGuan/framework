using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;

namespace Albatross.Templating {
	public interface IStringInterpolationService {
		string Interpolate<T>(string input, Func<string, T, string> func, T value);
	}

	public class StringInterpolationService : IStringInterpolationService{
		public StringInterpolationService(ILogger<StringInterpolationService> logger) {
			this.logger = logger;
		}
		/// <summary>
		/// the pattern looks for text within the "${" and "}" with no space allowed at its beginning or the end.
		/// </summary>
		public const string ExpressionSearchPattern = @"\$\{(?!\s)(.+?)(?<![\s])}";
		public static readonly Regex ExpressionSearchRegex = new Regex(ExpressionSearchPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
		private readonly ILogger<StringInterpolationService> logger;

		public string Interpolate<T>(string input, Func<string, T, string> func, T value) {
			return ExpressionSearchRegex.Replace(input, (match) => {
				string expression = match.Groups[1].Value;
				try {
					return func(expression, value);
				} catch (Exception err) {
					logger.LogError(err, "expression parsing exception: {expression}", expression);
					return match.Groups[0].Value;
				}
			});
		}
	}
}
