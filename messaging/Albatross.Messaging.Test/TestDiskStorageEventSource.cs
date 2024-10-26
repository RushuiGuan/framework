using Albatross.Messaging.Configurations;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Albatross.Messaging.Test {
	public class TestDiskStorageEventSource {
		private readonly ITestOutputHelper testOutputHelper;

		public TestDiskStorageEventSource(ITestOutputHelper testOutputHelper) {
			this.testOutputHelper = testOutputHelper;
		}

		ILogger Logger { get => new Mock<ILogger>().Object; }
		ILoggerFactory LoggerFactory {
			get {
				var mock = new Mock<ILoggerFactory>();
				mock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(Logger);
				return mock.Object;
			}
		}
		DiskStorageEventWriter GetWriter(string name, DiskStorageConfiguration config) {
			return new DiskStorageEventWriter(name, config, LoggerFactory);
		}

		IEventReader GetReader(DiskStorageConfiguration config) {
			return new DiskStorageEventReader(config, new Mock<IMessageFactory>().Object, new Mock<ILogger>().Object);
		}

		IMessageFactory MessageFactory { get => new Mock<IMessageFactory>().Object; }

		void CleanFolder(string name) {
			if (Directory.Exists(name)) {
				Directory.Delete(name, true);
			}
		}

		/// <summary>
		/// Test file creation by max file size
		/// </summary>
		[Theory]
		[InlineData(25, 10, 11)]
		[InlineData(1000, 50, 2)]
		public async Task TestOutputFileSizeCheck(int maxFileSize, int messageCount, int expectedFileCount) {
			var folder = $"c:\\temp\\{nameof(TestOutputFileSizeCheck)}\\{maxFileSize}-{messageCount}-{expectedFileCount}";
			CleanFolder(folder);

			var config = new DiskStorageConfiguration(folder, "test-output") {
				MaxFileSize = maxFileSize,
			};
			using (var writer = GetWriter("test1", config)) {
				int count = messageCount;
				for (int i = 0; i < count; i++) {
					writer.WriteEvent(new EventEntry(EntryType.Record, new TestMsg()));
					await Task.Delay(100);
				}
			}
			Assert.Equal(expectedFileCount, Directory.GetFiles(folder).Length);
		}

		[Theory]
		[InlineData(25, 10, 0)]
		[InlineData(1000, 50, 0)]
		[InlineData(10000, 50, 10)]
		[InlineData(10000, 100, 99)]
		[InlineData(10000, 100, 98)]
		[InlineData(1000, 100, 99)]
		[InlineData(1000, 100, 98)]
		public async Task TestReadOverMultipleFiles(int maxFileSize, int messageCount, int readMarkerIndex) {
			var folder = $"c:\\temp\\{nameof(TestReadOverMultipleFiles)}_{maxFileSize}_{messageCount}_{readMarkerIndex}";
			CleanFolder(folder);
			var config = new DiskStorageConfiguration(folder, "test-output") {
				MaxFileSize = maxFileSize,
			};
			using (var writer = GetWriter("test1", config)) {
				int count = messageCount;
				DateTime readMarker = DateTime.Now;
				for (int i = 0; i < count; i++) {
					if (i == readMarkerIndex) {
						readMarker = DateTime.Now;
						// introduce some buffer
						await Task.Delay(100);
					}
					writer.WriteEvent(new EventEntry(EntryType.Record, new TestMsg()));
					await Task.Delay(1);
				}
				var reader = new DiskStorageEventReader(config, MessageFactory, Logger);
				await Task.Delay(100);
				var duration = DateTime.Now - readMarker;
				var items = reader.ReadLast(duration);
				testOutputHelper.WriteLine("read marker: {0:yyyy-MM-ddTHH:mm:ss:fff}", readMarker);
				testOutputHelper.WriteLine("first read entry: {0:yyyy-MM-ddTHH:mm:ss:fff}", items.First().TimeStamp);
				testOutputHelper.WriteLine("last read entry: {0:yyyy-MM-ddTHH:mm:ss:fff}", items.Last().TimeStamp);
				Assert.Equal(messageCount - readMarkerIndex, items.Count());
			}
		}
	}
}