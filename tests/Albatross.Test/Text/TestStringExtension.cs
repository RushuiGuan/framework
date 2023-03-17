using Albatross.Text;
using System;
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
	}
}
