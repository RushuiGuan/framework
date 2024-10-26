using Albatross.Messaging.PubSub.Messages;
using NetMQ;
using System.IO;
using Xunit;

namespace Albatross.Messaging.Test {
	public class TestPubSubMessages {
		[Theory]
		[InlineData(true, "dafkda", 9393, "mytopic", "mypattern", "mydata")]
		[InlineData(false, "dafkda", 9393, "mytopic", "mypattern", "mydata")]
		public void ReadWriteEvent(bool useText, string route, ulong id, string topic, string pattern, string data) {
			var msg = new Event(route, id, topic, pattern, data.ToUtf8Bytes());
			var result = new Event();
			if (useText) {
				StringWriter writer = new StringWriter();
				msg.WriteToText(writer);
				int offset = 0;
				result.ReadFromText(writer.ToString(), ref offset);
			} else {
				var frames = new NetMQMessage();
				msg.WriteToFrames(frames);
				result.ReadFromFrames(frames);
			}
			Assert.Equal(msg.Header, result.Header);
			Assert.Equal(msg.Route, result.Route);
			Assert.Equal(msg.Id, result.Id);
			Assert.Equal(msg.Topic, result.Topic);
			Assert.Equal(msg.Pattern, result.Pattern);
			Assert.Equal(msg.Payload, result.Payload);
		}

		[Theory]
		[InlineData(true, "dafkda", 9393, true, "mypattern")]
		[InlineData(true, "dafkda", 9393, false, "mypattern")]
		[InlineData(false, "dafkda", 9393, true, "mypattern")]
		[InlineData(false, "dafkda", 9393, false, "mypattern")]
		public void ReadWriteSubscriptionReply(bool useText, string route, ulong id, bool on, string pattern) {
			var msg = new SubscriptionReply(route, id, on, pattern);
			var result = new SubscriptionReply();
			if (useText) {
				StringWriter writer = new StringWriter();
				msg.WriteToText(writer);
				int offset = 0;
				result.ReadFromText(writer.ToString(), ref offset);
			} else {
				var frames = new NetMQMessage();
				msg.WriteToFrames(frames);
				result.ReadFromFrames(frames);
			}
			Assert.Equal(msg.Header, result.Header);
			Assert.Equal(msg.Route, result.Route);
			Assert.Equal(msg.Id, result.Id);
			Assert.Equal(msg.Pattern, result.Pattern);
			Assert.Equal(msg.On, result.On);
		}

		[Theory]
		[InlineData(true, "dafkda", 9393, true, "mypattern")]
		[InlineData(true, "dafkda", 9393, false, "mypattern")]
		[InlineData(false, "dafkda", 9393, true, "mypattern")]
		[InlineData(false, "dafkda", 9393, false, "mypattern")]
		public void ReadWriteSubscriptionRequest(bool useText, string route, ulong id, bool on, string pattern) {
			var msg = new SubscriptionRequest(route, id, on, pattern);
			var result = new SubscriptionRequest();
			if (useText) {
				StringWriter writer = new StringWriter();
				msg.WriteToText(writer);
				int offset = 0;
				result.ReadFromText(writer.ToString(), ref offset);
			} else {
				var frames = new NetMQMessage();
				msg.WriteToFrames(frames);
				result.ReadFromFrames(frames);
			}
			Assert.Equal(msg.Header, result.Header);
			Assert.Equal(msg.Route, result.Route);
			Assert.Equal(msg.Id, result.Id);
			Assert.Equal(msg.Pattern, result.Pattern);
			Assert.Equal(msg.On, result.On);
		}

		[Theory]
		[InlineData(true, "dafkda", 9393)]
		[InlineData(false, "dafkda", 9393)]
		public void ReadWriteUnsubscribeAllRequest(bool useText, string route, ulong id) {
			var msg = new UnsubscribeAllRequest(route, id);
			var result = new UnsubscribeAllRequest();
			if (useText) {
				StringWriter writer = new StringWriter();
				msg.WriteToText(writer);
				int offset = 0;
				result.ReadFromText(writer.ToString(), ref offset);
			} else {
				var frames = new NetMQMessage();
				msg.WriteToFrames(frames);
				result.ReadFromFrames(frames);
			}
			Assert.Equal(msg.Header, result.Header);
			Assert.Equal(msg.Route, result.Route);
			Assert.Equal(msg.Id, result.Id);
		}
	}
}