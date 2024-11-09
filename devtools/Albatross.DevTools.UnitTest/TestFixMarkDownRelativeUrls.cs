using Xunit;

namespace Albatross.DevTools.UnitTest {
	public class TestFixMarkDownRelativeUrls {
		[Theory]
		[InlineData("C:\\root", "C:\\root\\file.md", "file.md")]
		[InlineData("C:\\root", "C:\\root\\folder\\file.md", "folder\\file.md")]
		[InlineData("C:\\root", "C:\\file.md", "..\\file.md")]
		[InlineData("C:\\root", "C:\\temp\\file.md", "..\\temp\\file.md")]
		public void TestGetRelativePath(string rootFolder, string file, string expected) {
			Assert.Equal(expected, FixMarkDownRelativeUrls.GetRelativeUrl(new DirectoryInfo(rootFolder), new FileInfo(file)));
		}

		[Theory]
		[InlineData("C:\\root", "E:\\temp\\file.md")]
		public void TestGetRelativePath_InvalidRootFolder(string rootFolder, string file) {
			Assert.Throws<ArgumentException>(() => FixMarkDownRelativeUrls.GetRelativeUrl(new DirectoryInfo(rootFolder), new FileInfo(file)));
		}

		[Theory]
		[InlineData("http://root", "file.md", "relative.md", "http://root/relative.md")]
		[InlineData("http://root", "files/file.md", "relative.md", "http://root/files/relative.md")]
		[InlineData("http://root", "files/file.md", "..", "http://root/")]
		[InlineData("http://root", "files/file.md", "../test", "http://root/test")]
		[InlineData("http://root", "files/file.md", "#bookmark", "http://root/files/file.md#bookmark")]
		public void TestGetAbsoluteUrl(string rootUrl, string relativeFilePath, string relativeUrl, string expected) {
			Assert.Equal(expected, FixMarkDownRelativeUrls.GetAbsoluteUrl(rootUrl, relativeFilePath, relativeUrl));
		}

		[Theory]
		[InlineData("http://root", "file.md", "http://root/relative.md")]
		[InlineData("http://root", "c:\\file.md", "files/relative.md")]
		// this case is invalid but can be skipped since relativeFilePath will be verified first
		// [InlineData("http://root", "..\\file.md", "files/relative.md")]
		public void TestGetAbsoluteUrl_InvalidRelativePath(string rootUrl, string relativeFilePath, string relativeUrl) {
			Assert.Throws<ArgumentException>(() => FixMarkDownRelativeUrls.GetAbsoluteUrl(rootUrl, relativeFilePath, relativeUrl));
		}

		[Theory]
		[InlineData("Reference [Albatross.CommandLine](.) from nuget", "c:\\app\\framework", "c:\\app\\framework\\commandline\\Albatross.CommandLine\\README.md", "https://repo", "Reference [Albatross.CommandLine](https://repo/commandline/Albatross.CommandLine/) from nuget")]
		public void TestReplaceAll(string text, string rootFolder, string markdownFile, string rootUrl, string expected) {
			Assert.Equal(expected, FixMarkDownRelativeUrls.ReplaceAll(text, new DirectoryInfo(rootFolder), new FileInfo(markdownFile), rootUrl));
		}
	}
}
