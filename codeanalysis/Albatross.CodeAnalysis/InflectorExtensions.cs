using System;
using System.Text.RegularExpressions;

namespace Humanizer {
	/// <summary>
	/// Copy from Humanizer <see href="https://github.com/Humanizr"/> because it is so far impossible for a codegen dll to reference other
	/// dlls.
	/// </summary>
	public static class InflectorExtensions {
		/// <summary>
		/// By default, pascalize converts strings to UpperCamelCase also removing underscores
		/// </summary>
		public static string Pascalize(this string input) =>
			Regex.Replace(input, @"(?:[ _-]+|^)([a-zA-Z])", match => match
				.Groups[1]
				.Value.ToUpper());


		/// <summary>
		/// Separates the input words with underscore
		/// </summary>
		/// <param name="input">The string to be underscored</param>
		public static string Underscore(this string input) =>
			Regex
				.Replace(
					Regex.Replace(
						Regex.Replace(input, @"([\p{Lu}]+)([\p{Lu}][\p{Ll}])", "$1_$2"), @"([\p{Ll}\d])([\p{Lu}])", "$1_$2"), @"[-\s]", "_")
				.ToLower();

		/// <summary>
		/// Replaces underscores with dashes in the string
		/// </summary>
		public static string Dasherize(this string underscoredWord) =>
			underscoredWord.Replace('_', '-');

		/// <summary>
		/// Replaces underscores with hyphens in the string
		/// </summary>
		public static string Hyphenate(this string underscoredWord) =>
			Dasherize(underscoredWord);

		/// <summary>
		/// Separates the input words with hyphens and all the words are converted to lowercase
		/// </summary>
		public static string Kebaberize(this string input) =>
			Underscore(input)
				.Dasherize();
	}
}
