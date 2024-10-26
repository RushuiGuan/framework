using Albatross.IO;
using Xunit;

namespace Albatross.IO.Test {
	public class TestIOUtility {

		[Fact]
		public void TestEnsureDirectory() {
			@"c:\temp\test\me\like".EnsureDirectory();
		}

		[Theory]
		[InlineData("abc-123", " ", "abc-123")]
		[InlineData("abc-123#", "-", "abc-123#")]
		[InlineData("test.txt", "-", "test.txt")]
		[InlineData("abc-123*", "", "abc-123")]
		public void TestConvertText2Filename(string text, string filler, string expected) {
			Assert.Equal(expected, text.ConvertToFilename(filler));
		}
	}
}