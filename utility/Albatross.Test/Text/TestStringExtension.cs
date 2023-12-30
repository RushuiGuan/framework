using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Albatross.Test.Text {
	public class TestStringExtension {
		[Theory]
		[InlineData(null, null)]
		[InlineData("","")]
		[InlineData(".", ".")]
		[InlineData("AB", "AB.")]
		[InlineData("AB.", "AB.")]
		public void TestPostfixIfNotNullOrEmpty(string text, string expected) {
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
			while(input.TryGetText(delimiter, ref offset, out var text)) {
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
	}
}
