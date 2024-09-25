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
			var sourceFile = My.SortedTestFile("0-9");
			var destinationFile = My.SortedTestFile("10-19");

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			destinationFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-19").StringContent());
			sourceFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-9").StringContent());
		}


		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependEmptySource(bool copySource) {
			var sourceFile = My.SortedTestFile("0-9");
			var destinationFile = My.SortedTestFile("10-19");

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					srcStream.Seek(0, SeekOrigin.End);
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			destinationFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile("10-19").StringContent());
			// source file should not change
			sourceFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-9").StringContent());
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependEmptyDestination(bool copySource) {
			var sourceFile = My.SortedTestFile("0-9");
			var destinationFile = My.SortedTestFile("10-19");

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					dstStream.Seek(0, SeekOrigin.End);
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			destinationFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-9").StringContent());
			// source file should not change
			sourceFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-9").StringContent());
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependHalfSource(bool copySource) {
			var sourceFile = My.SortedTestFile("0-9");
			var destinationFile = My.SortedTestFile("10-19");

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					for(int i=0; i<5; i++) {
						srcStream.TryReadLine(out _, out _);
					}
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			destinationFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile("5-19").StringContent());
			// source file should not change
			sourceFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-9").StringContent());
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependHalfDestination(bool copySource) {
			var sourceFile = My.SortedTestFile("0-9");
			var destinationFile = My.SortedTestFile("10-19");

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					for (int i = 0; i < 5; i++) {
						dstStream.TryReadLine(out _, out _);
					}
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			destinationFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-9,15-19").StringContent());
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependPartialSourceAndPartialDestination(bool copySource) {
			var sourceFile = My.SortedTestFile("0-9");
			var destinationFile = My.SortedTestFile("10-19");

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					for (int i = 0; i < 5; i++) {
						dstStream.TryReadLine(out _, out _);
						srcStream.TryReadLine(out _, out _);
					}
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			destinationFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile("5-9,15-19").StringContent());
			// source file should not change
			sourceFile.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-9").StringContent());
		}
	}
}
