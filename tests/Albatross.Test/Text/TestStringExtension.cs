using Albatross.Text;
using System;
using System.IO;
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
	}
}
