using Albatross.Messaging.Commands.Messages;
using NetMQ;
using System.IO;
using Xunit;

namespace Albatross.Messaging.Test {
	public class TestCommandMessages {

		[Theory]
		[InlineData(true, "xxabc", 99, "mytype", "myerror", "myerrormsg")]
		[InlineData(false, "xxabc", 99, "mytype", "myerror", "myerrormsg")]
		public void ReadWriteCommandErrorReply(bool useText, string route, ulong id, string commandType, string errorClassName, string message) {
			var msg = new CommandErrorReply(route, id, commandType, errorClassName, message.ToUtf8Bytes());
			var result = new CommandErrorReply();
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
			Assert.Equal(msg.Message, result.Message);
			Assert.Equal(msg.Route, result.Route);
			Assert.Equal(msg.Id, result.Id);
			Assert.Equal(msg.CommandName, result.CommandName);
			Assert.Equal(msg.ClassName, result.ClassName);
		}

		[Theory]
		[InlineData(true, "xxabc", 99, "mytype", "mydata")]
		[InlineData(false, "xxabc", 99, "mytype", "mydata")]
		public void ReadWriteCommandReply(bool useText, string route, ulong id, string commandType, string message) {
			var msg = new CommandReply(route, id, commandType, message.ToUtf8Bytes());
			var result = new CommandReply();
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
			Assert.Equal(msg.Payload, result.Payload);
			Assert.Equal(msg.Route, result.Route);
			Assert.Equal(msg.Id, result.Id);
			Assert.Equal(msg.CommandName, result.CommandName);
		}

		[Theory]
		[InlineData(true, "xxabc", 99, "mytype", CommandMode.Internal, "mydata")]
		[InlineData(true, "xxabc", 99, "mytype", CommandMode.Callback, "mydata")]
		[InlineData(true, "xxabc", 99, "mytype", CommandMode.FireAndForget, "mydata")]
		[InlineData(false, "xxabc", 99, "mytype", CommandMode.Internal, "mydata")]
		[InlineData(false, "xxabc", 99, "mytype", CommandMode.Callback, "mydata")]
		[InlineData(false, "xxabc", 99, "mytype", CommandMode.FireAndForget, "mydata")]
		public void ReadWriteCommandRequest(bool useText, string route, ulong id, string commandName, CommandMode mode, string message) {
			var msg = new CommandRequest(route, id, commandName, mode, message.ToUtf8Bytes());
			var result = new CommandRequest();
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
			Assert.Equal(msg.CommandName, result.CommandName);
			Assert.Equal(msg.Mode, result.Mode);
			Assert.Equal(msg.Payload, result.Payload);
		}

		[Theory]
		[InlineData(true, "xxabc", 99)]
		[InlineData(false, "xxabc", 99)]
		public void ReadWriteCommandRequestAck(bool useText, string route, ulong id) {
			var msg = new CommandRequestAck(route, id);
			var result = new CommandRequestAck();
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