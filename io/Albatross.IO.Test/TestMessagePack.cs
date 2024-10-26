using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.IO.Test {
	[MessagePackObject]
	public class MyMessage {
		[Key(0)]
		public DateTime Position { get; set; }
		[Key(1)]
		public DateTime TimeStamp { get; set; }
	}

	[MessagePack.MessagePackObject]
	public record class OneMinuteBar {
		[MessagePack.Key(0)]
		public DateTime DateTimeUtc { get; set; }
		// 16*6 = 96 + 8 = 104 + 8 = 122
		[MessagePack.Key(1)]
		public decimal Open { get; set; }
		[MessagePack.Key(2)]
		public decimal High { get; set; }
		[MessagePack.Key(3)]
		public decimal Low { get; set; }
		[MessagePack.Key(4)]
		public decimal Last { get; set; }
		[MessagePack.Key(5)]
		public int Volume { get; set; }
		[MessagePack.Key(6)]
		public decimal VWAP { get; set; }
		[MessagePack.Key(7)]
		public decimal TWAP { get; set; }
		public OneMinuteBar() { }
	}

	public class TestMessagePack {
		MessagePackSerializerOptions DefaultNoCompression = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.None);
		MessagePackSerializerOptions DefaultWithCompression = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);

		MessagePackSerializerOptions NativeNoCompression = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.None)
			.WithResolver(CompositeResolver.Create(NativeDateTimeResolver.Instance, NativeDecimalResolver.Instance, StandardResolver.Instance));

		MessagePackSerializerOptions NativeWithCompression = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block)
			.WithResolver(CompositeResolver.Create(NativeDateTimeResolver.Instance, NativeDecimalResolver.Instance, StandardResolver.Instance));

		[Fact]
		public void NormalOperation() {
			var stream = new MemoryStream();
			var now = DateTime.UtcNow;

			MessagePackSerializer.Serialize(stream, new OneMinuteBar() { DateTimeUtc = now, }, DefaultWithCompression);
			stream.Position = 0;
			var msg = MessagePackSerializer.Deserialize<OneMinuteBar>(stream, DefaultWithCompression);
			Assert.Equal(now, msg.DateTimeUtc);
		}

		[Fact]
		public void TestFileIndex() {
			var index = new List<FileIndexValue<DateTime>>();
			for (int i = 0; i < 100; i++) {
				index.Add(DateTime.Now, i);
			}
			var stream = new MemoryStream();
			try {
				MessagePackSerializer.Serialize(stream, index, DefaultWithCompression);
			} catch (Exception e) {
				Console.WriteLine(e);
			}
			stream.Position = 0;
			var index2 = MessagePackSerializer.Deserialize<List<FileIndexValue<DateTime>>>(stream, DefaultWithCompression);
			Assert.Equal(100, index2.Count);
		}

		[Fact]
		public void TestStreamLength() {
			var index = new List<FileIndexValue<DateTime>>();
			for (int i = 0; i < 1000; i++) {
				index.Add(DateTime.Now, i);
			}
			var stream = new MemoryStream();
			MessagePackSerializer.Serialize(stream, index, DefaultNoCompression);
			Assert.True(stream.Length == stream.Position);
			var original = stream.Position;
			stream.Position = 0;

			index.Clear();
			for (int i = 0; i < 50; i++) {
				index.Add(DateTime.Now, i);
			}
			MessagePackSerializer.Serialize(stream, index, DefaultNoCompression);
			var newLength = stream.Position;
			// new length should be less than the original length
			Assert.True(original > newLength);
			// messagepack does not shrink the stream
			Assert.Equal(original, stream.Length);
		}

		[Fact]
		public async Task StreamOperation() {
			var stream = new MemoryStream();
			for (int i = 0; i < 100; i++) {
				MessagePackSerializer.Serialize(stream, new MyMessage() { Position = DateTime.Now, TimeStamp = DateTime.Now });
			}
			var list = new List<MyMessage>();
			stream.Position = 0;
			using var reader = new MessagePackStreamReader(stream);
			for (var value = await reader.ReadAsync(CancellationToken.None); value != null; value = await reader.ReadAsync(CancellationToken.None)) {
				var msg = MessagePackSerializer.Deserialize<MyMessage>(value.Value);
				Assert.NotNull(msg);
				list.Add(msg);
			}
			Assert.Equal(100, list.Count);
		}
	}
}