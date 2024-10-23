using Xunit;
using MessagePack;
using System;
using System.Collections.Generic;

namespace Albatross.IO.Test {
	[MessagePackObject]
	public class MyMessage1 { 
		[Key(0)]
		public DateTime TimeStamp { get; set; }
		[Key(1)]
		public long Position { get; set; }
	}

	public class TestMessagePack {
		[Fact]
		public void NormalOperation() {
			var list = new List<MyMessage1>();
			var dateTime = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc);
			for (int i = 0; i < 100; i++) {
				list.Add(new MyMessage1() { TimeStamp = dateTime, Position = i });
			}
			byte[] bytes = MessagePackSerializer.Serialize(list);
			var result = MessagePackSerializer.Deserialize<MyMessage1[]>(bytes);
			Assert.Equal(100, result.Length);
			int count = 0;
			foreach (var item in result) {
				Assert.Equal(dateTime, item.TimeStamp);
				Assert.Equal(DateTimeKind.Utc, item.TimeStamp.Kind);
				Assert.Equal(count, item.Position);
				count++;
			}
		}
	}
}
