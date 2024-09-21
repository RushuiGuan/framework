using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.IO.Test {
	public class TestASCIIFilePrepend {
		string GetTestFile(int start, int count, string? fileName = null) {
			var file = fileName ?? Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				for (int i = start; i < start + count; i++) {
					writer.WriteLine(i);
				}
			}
			return file;
		}


		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrepend(bool copySource) {
			var sourceFile = GetTestFile(0, 10);
			var destinationFile = GetTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					await dstStream.Prepend(srcStream, copySource);
				}
			}

			var verify = GetTestFile(0, 20);
			(await File.ReadAllBytesAsync(destinationFile))
				.Should().Equal(await File.ReadAllBytesAsync(verify));
			// source file should not change
			(await File.ReadAllBytesAsync(sourceFile))
				.Should().Equal(await File.ReadAllBytesAsync(GetTestFile(0, 10)));
		}


		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependEmptySource(bool copySource) {
			var sourceFile = GetTestFile(0, 10);
			var destinationFile = GetTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					srcStream.Seek(0, SeekOrigin.End);
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			var verify = GetTestFile(10, 10);
			(await File.ReadAllBytesAsync(destinationFile))
				.Should().Equal(await File.ReadAllBytesAsync(verify));
			// source file should not change
			(await File.ReadAllBytesAsync(sourceFile))
				.Should().Equal(await File.ReadAllBytesAsync(GetTestFile(0, 10)));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependEmptyDestination(bool copySource) {
			var sourceFile = GetTestFile(0, 10);
			var destinationFile = GetTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					dstStream.Seek(0, SeekOrigin.End);
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			var verify = GetTestFile(0, 10);
			(await File.ReadAllBytesAsync(destinationFile))
				.Should().Equal(await File.ReadAllBytesAsync(verify));
			// source file should not change
			(await File.ReadAllBytesAsync(sourceFile))
				.Should().Equal(await File.ReadAllBytesAsync(GetTestFile(0, 10)));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependHalfSource(bool copySource) {
			var sourceFile = GetTestFile(0, 10);
			var destinationFile = GetTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					for(int i=0; i<5; i++) {
						srcStream.TryReadLineFromStream(out _, out _);
					}
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			var verify = GetTestFile(5, 15);
			(await File.ReadAllBytesAsync(destinationFile))
				.Should().Equal(await File.ReadAllBytesAsync(verify));
			// source file should not change
			(await File.ReadAllBytesAsync(sourceFile))
				.Should().Equal(await File.ReadAllBytesAsync(GetTestFile(0, 10)));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependHalfDestination(bool copySource) {
			var sourceFile = GetTestFile(0, 10);
			var destinationFile = GetTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					for (int i = 0; i < 5; i++) {
						dstStream.TryReadLineFromStream(out _, out _);
					}
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			var verify = await GetTestFile(0, 10).Append(GetTestFile(15, 5));
			(await File.ReadAllBytesAsync(destinationFile))
				.Should().Equal(await File.ReadAllBytesAsync(verify));
			// source file should not change
			(await File.ReadAllBytesAsync(sourceFile))
				.Should().Equal(await File.ReadAllBytesAsync(GetTestFile(0, 10)));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task TestPrependPartialSourceAndPartialDestination(bool copySource) {
			var sourceFile = GetTestFile(0, 10);
			var destinationFile = GetTestFile(10, 10);

			using (var srcStream = File.Open(sourceFile, FileMode.Open, FileAccess.ReadWrite)) {
				using (var dstStream = File.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite)) {
					for (int i = 0; i < 8; i++) {
						dstStream.TryReadLineFromStream(out _, out _);
						srcStream.TryReadLineFromStream(out _, out _);
					}
					await dstStream.Prepend(srcStream, copySource);
				}
			}
			var verify = await GetTestFile(8, 2).Append(GetTestFile(18, 2));
			(await File.ReadAllBytesAsync(destinationFile))
				.Should().Equal(await File.ReadAllBytesAsync(verify));
			// source file should not change
			(await File.ReadAllBytesAsync(sourceFile))
				.Should().Equal(await File.ReadAllBytesAsync(GetTestFile(0, 10)));
		}
	}
}
