using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.IO.Test {
	public class TestFileStreamRetryPolicy {
		[Fact]
		public async Task TestFileNotFoundException() {
			var file = new FileInfo("a file that does not exist.txt");
			var logger = new Mock<ILogger>().Object;
			await Assert.ThrowsAsync<FileNotFoundException>(() => file.OpenSharedReadStreamWithRetry(4096, 3, 100, FileOptions.None, logger));
		}

		[Fact]
		public async Task TestFileSharedRead_NormalUseCase() {
			var text = DateTime.Now.Ticks.ToString();
			var path = CreateTestFile(text);
			var logger = new Mock<ILogger>().Object;
			var file = new FileInfo(path);
			int? retry1 = null, retry2 = null;
			using (var stream1 = await file.OpenSharedReadStreamWithRetry(4096, 3, 100, FileOptions.None, logger, x => retry1 = x)) {
				using (var stream2 = await file.OpenSharedReadStreamWithRetry(4096, 3, 100, FileOptions.None, logger, x => retry2 = x)) {
					using var reader1 = new StreamReader(stream1);
					using var reader2 = new StreamReader(stream2);
					var line1 = await reader1.ReadLineAsync();
					var line2 = await reader2.ReadLineAsync();
					Assert.Equal(text, line1);
					Assert.Equal(text, line2);
				}
			}
			Assert.Null(retry1);
			Assert.Null(retry2);
		}

		[Fact]
		public async Task TestFileRead_RetryUseCase() {
			var text = DateTime.Now.Ticks.ToString();
			var path = CreateTestFile(text);
			var logger = new Mock<ILogger>().Object;
			var file = new FileInfo(path);

			var writeStream = await file.OpenExclusiveReadWriteStreamWithRetry(4096, 3, 100, FileOptions.None, logger);
			// wait a second and dispose the write stream
			_ = Task.Delay(800).ContinueWith(x => writeStream.Dispose());
			int retryCount = 0;
			using (var readStream = await file.OpenSharedReadStreamWithRetry(4096, 10, 300, FileOptions.None, logger, x => retryCount = x)) {
				using var reader2 = new StreamReader(readStream);
				var line2 = await reader2.ReadLineAsync();
				Assert.Equal(text, line2);
			}
			Assert.Equal(3, retryCount);
		}

		string CreateTestFile(string text) {
			var file = Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				writer.WriteLine(text);
			}
			return file;
		}
	}
}