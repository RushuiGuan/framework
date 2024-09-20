using System;
using Xunit;
using Albatross.IO;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Albatross.Test.IO {
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


		[Fact]
		public async Task TestPrepend() {
			//var sourceFile = Path.GetTempFileName();
			//var destinationFile = Path.GetTempFileName();
			var sourceFile = @"c:\temp\source.txt";
			var destinationFile = @"c:\temp\destination.txt";
			using (var writer = new StreamWriter(sourceFile)) {
				for (int i = 0; i < 10; i++) {
					writer.WriteLine(i);
				}
			}
			using(var writer = new StreamWriter(destinationFile)) {
				for (int i = 10; i < 20; i++) {
					writer.WriteLine(i);
				}
			}
			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using(var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					await dstStream.Prepend(srcStream);
				}
			}

			using(var reader = new StreamReader(destinationFile)) {
				var lines = new List<string>();
				while (!reader.EndOfStream) {
					var line = await reader.ReadLineAsync();
					lines.Add(line??string.Empty);
				}
				Assert.Equal(20, lines.Count);
				for(int i=0; i < 20; i++) {
					Assert.Equal(i.ToString(), lines[i]);
				}
			}
		}
	}
}
