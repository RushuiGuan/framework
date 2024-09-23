using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.IO.Test {
	public class TestASCIIFilePrepend {
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrepend(bool copySource) {
			var sourceFile = My.SortedTestFile(0, 10);
			var destinationFile = My.SortedTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			destinationFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile(0, 20).StringContent());
			sourceFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile(0, 10).StringContent());
		}


		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependEmptySource(bool copySource) {
			var sourceFile = My.SortedTestFile(0, 10);
			var destinationFile = My.SortedTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					srcStream.Seek(0, SeekOrigin.End);
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			destinationFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile(10, 10).StringContent());
			// source file should not change
			sourceFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile(0, 10).StringContent());
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependEmptyDestination(bool copySource) {
			var sourceFile = My.SortedTestFile(0, 10);
			var destinationFile = My.SortedTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					dstStream.Seek(0, SeekOrigin.End);
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			destinationFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile(0, 10).StringContent());
			// source file should not change
			sourceFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile(0, 10).StringContent());
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependHalfSource(bool copySource) {
			var sourceFile = My.SortedTestFile(0, 10);
			var destinationFile = My.SortedTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					for(int i=0; i<5; i++) {
						srcStream.TryReadLine(out _, out _);
					}
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			destinationFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile(5, 15).StringContent());
			// source file should not change
			sourceFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile(0, 10).StringContent());
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependHalfDestination(bool copySource) {
			var sourceFile = My.SortedTestFile(0, 10);
			var destinationFile = My.SortedTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					for (int i = 0; i < 5; i++) {
						dstStream.TryReadLine(out _, out _);
					}
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			var verify = await My.SortedTestFile(0, 10).Append(My.SortedTestFile(15, 5));
			destinationFile.StringContent().Should().BeEquivalentTo(verify.StringContent());
			// source file should not change
			sourceFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile(0, 10).StringContent());
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependPartialSourceAndPartialDestination(bool copySource) {
			var sourceFile = My.SortedTestFile(0, 10);
			var destinationFile = My.SortedTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					for (int i = 0; i < 8; i++) {
						dstStream.TryReadLine(out _, out _);
						srcStream.TryReadLine(out _, out _);
					}
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			var verify = await My.SortedTestFile(8, 2).Append(My.SortedTestFile(18, 2));
			destinationFile.StringContent().Should().BeEquivalentTo(verify.StringContent());
			// source file should not change
			sourceFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile(0, 10).StringContent());
		}
	}
}
