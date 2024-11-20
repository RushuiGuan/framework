using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Moq;
using System;
using Xunit;

namespace Albatross.Messaging.Test {
	public class TestEventEntryReadWrite {
		[Theory]
		[InlineData("R 2024-10-22T16:08:33.030Z cmd-rep pricemaster-api-daemon_RGH11 1138784 PriceMaster.Core.Commands.ProcessTickWriteDeepHistoryFileCommand,PriceMaster.Core", true, "2024-10-22T16:08:33.030")]
		public void TestEntryTimeStampParsing(string line, bool expected_parsed, string expected_timestamp_text) {
			var mock = new Mock<IMessageFactory>();
			mock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<int>())).Returns(() => new Mock<IMessage>().Object);
			var parsed = EventEntry.TryParseLine(mock.Object, line, out var replay);
			Assert.Equal(expected_parsed, parsed);
			if (parsed) {
				var expectedTimeStamp = DateTime.ParseExact(expected_timestamp_text, "yyyy-MM-ddTHH:mm:ss.fff", null);
				expectedTimeStamp = DateTime.SpecifyKind(expectedTimeStamp, DateTimeKind.Utc);
				Assert.Equal(expectedTimeStamp, replay.TimeStamp);
			}
		}
	}
}
