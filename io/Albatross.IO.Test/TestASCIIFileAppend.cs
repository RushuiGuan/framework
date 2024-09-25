using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.IO.Test {
	public class TestASCIIFileAppend {
		[Fact]
		public async Task TestAppend() {
			var current = My.SortedTestFile("0-9");
			var source = My.SortedTestFile("10-19");


			using (var srcStream = File.Open(source, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(current, FileMode.Open, FileAccess.ReadWrite)) {
					dstStream.Seek(0, SeekOrigin.End);
					await dstStream.Append(srcStream);
				}
			}
			current.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-19").StringContent());
		}


		[Fact]
		public async Task TestAppendEmptySource() {
			var source = My.SortedTestFile("10-19");
			var current = My.SortedTestFile("0-9");

			using (var srcStream = File.Open(source, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(current, FileMode.Open, FileAccess.ReadWrite)) {
					dstStream.Seek(0, SeekOrigin.End);
					srcStream.Seek(0, SeekOrigin.End);
					await dstStream.Append(srcStream);
				}
			}
			current.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-9").StringContent());
		}

		[Fact]
		public async Task TestAppendEmptyDestination() {
			var source = My.SortedTestFile("10-19");
			var current = My.SortedTestFile("0-9");

			using (var srcStream = File.Open(source, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(current, FileMode.Open, FileAccess.ReadWrite)) {
					srcStream.Seek(0, SeekOrigin.End);
					dstStream.Seek(0, SeekOrigin.End);
					await dstStream.Append(srcStream);
				}
			}
			current.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-9").StringContent());
		}

		[Fact]
		public async Task TestAppendHalfSource() {
			var source = My.SortedTestFile("10-19");
			var current = My.SortedTestFile("0-9");

			using (var srcStream = File.Open(source, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(current, FileMode.Open, FileAccess.ReadWrite)) {
					for(int i=0; i<5; i++) {
						srcStream.TryReadLine(out _, out _);
					}
					dstStream.Seek(0, SeekOrigin.End);
					await dstStream.Append(srcStream);
				}
			}
			current.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-9,15-19").StringContent());
		}

		[Fact]
		public async Task TestAppendHalfDestination() {
			var current = My.SortedTestFile("0-9");
			var source = My.SortedTestFile("10-19");

			using (var srcStream = File.Open(source, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(current, FileMode.Open, FileAccess.ReadWrite)) {
					for (int i = 0; i < 5; i++) {
						dstStream.TryReadLine(out _, out _);
					}
					await dstStream.Append(srcStream);
				}
			}
			current.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-4,10-19").StringContent());
		}

		[Fact]
		public async Task TestAppendPartialSourceAndPartialDestination() {
			var current = My.SortedTestFile("0-9");
			var source = My.SortedTestFile("10-19");

			using (var srcStream = File.Open(source, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(current, FileMode.Open, FileAccess.ReadWrite)) {
					for (int i = 0; i < 5; i++) {
						dstStream.TryReadLine(out _, out _);
						srcStream.TryReadLine(out _, out _);
					}
					await dstStream.Append(srcStream);
				}
			}
			current.StringContent().Should().BeEquivalentTo(My.SortedTestFile("0-4,15-19").StringContent());
		}
	}
}
