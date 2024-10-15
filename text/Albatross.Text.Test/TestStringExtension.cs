using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Albatross.Test.Text {
	public class TestStringExtension {
		[Theory]
		[InlineData(null, null)]
		[InlineData("", "")]
		[InlineData(".", ".")]
		[InlineData("AB", "AB.")]
		[InlineData("AB.", "AB.")]
		public void TestPostfixIfNotNullOrEmpty(string? text, string? expected) {
			var result = text.PostfixIfNotNullOrEmpty('.');
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("a.b", '.')]
		[InlineData("a.b.c", '.')]
		public void TestAppendJoin(string text, char delimiter) {
			var array = text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
			var result = new StringBuilder().AppendJoin(delimiter, array).ToString();
			Assert.Equal(text, result);
		}

		[Theory]
		[InlineData("a  b c d", ' ', "a..b.c.d")]
		[InlineData("a b c d", ' ', "a.b.c.d")]
		[InlineData("abcd", ' ', "abcd")]
		public void TestTryGetText(string input, char delimiter, string expected) {
			List<string> list = new List<string>();
			int offset = 0;
			while (input.TryGetText(delimiter, ref offset, out var text)) {
				list.Add(text);
			}
			var result = string.Join('.', list);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("abcd", "abcd", ' ', 'x')]
		[InlineData(" bcd", "abcd", ' ', 'a')]
		public void TestReplaceMultiCharacters(string expected, string input, char replacedWith, params char[] targets) {
			var result = input.ReplaceMultipleChars(replacedWith, targets);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(",", "a,b,c,d", null, null, "a,b,c,d")]
		[InlineData(".", "a,b,c,d", "---", "***", "---a.b.c.d***")]
		[InlineData(".", "", "---", "***", "")]
		[InlineData(".", "a,b,,d", "---", "***", "---a.b.d***")]
		public void TestWriteItems(string delimiter, string data, string? prefix, string? postfix, string expected) {
			var array = data.Split(",", StringSplitOptions.None).Select(x=> x == "" ? null : x).ToArray();
			var result = new StringWriter().WriteItems(array, delimiter, null, prefix, postfix).ToString();
			Assert.Equal(expected, result);

			result = new StringWriter().WriteItems(array, delimiter, (w, t) => w.Write(t), prefix, postfix).ToString();
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("", "a", "")]
		[InlineData("", "", "")]
		[InlineData("a", "a", "")]
		[InlineData("ab", "a", "b")]
		[InlineData("ab", "", "ab")]
		[InlineData("abc", "ab", "c")]
		[InlineData("abc", "abc", "")]
		public void TestTrimStart(string text, string trim, string expected) {
			var result = text.TrimStart(trim);
			Assert.Equal(expected, result);
		}
		[Theory]
		[InlineData("", "a", "")]
		[InlineData("", "", "")]
		[InlineData("a", "a", "")]
		[InlineData("ab", "b", "a")]
		[InlineData("ab", "", "ab")]
		[InlineData("abc", "bc", "a")]
		[InlineData("abc", "abc", "")]
		public void TestTrimEnd(string text, string trim, string expected) {
			var result = text.TrimEnd(trim);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("0", "0")]
		[InlineData("0.0", "0")]
		[InlineData("0.1", "0.1")]
		[InlineData("0.10", "0.1")]
		[InlineData("0.100", "0.1")]
		[InlineData("0.123", "0.123")]
		[InlineData(".0", "0")]
		[InlineData("0.", "0")]
		[InlineData("1.0", "1")]
		[InlineData("1", "1")]
		[InlineData("10.0101010", "10.010101")]
		[InlineData("10.123", "10.123")]
		[InlineData("10.1230", "10.123")]
		[InlineData("10.1230000", "10.123")]
		[InlineData("10", "10")]
		[InlineData("123", "123")]
		public void TestTrimDecimal(string number, string expected) {
			var value = decimal.Parse(number);
			var result = value.TrimDecimal();
			Assert.Equal(expected, result);
		}
	}
}
