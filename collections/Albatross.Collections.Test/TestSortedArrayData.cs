using FluentAssertions;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Albatross.Collections.Test {
	public class TestSortedArrayData {
		[Theory]
		[InlineData(0, 10, 0, 9)]
		[InlineData(7, 2, 7, 8)]
		[InlineData(0, 0, 0, 0)]
		[InlineData(1, 1, 1, 1)]
		[InlineData(100, 1, 100, 100)]
		public void FirstKeyLastKey(int start, int count, int first, int last) {
			var data = new SortedArrayData<int, int>(My.Array(start, count), x => x);
			data.FirstKey.Should().Be(first);
			data.LastKey.Should().Be(last);
		}

		[Theory]
		[InlineData(0, 0, "", "")]
		[InlineData(0, 5, "0,1,2,3,4", "0,1,2,3,4")]
		[InlineData(1, 5, "1,2,3,4,5", "0,1,2,3,4")]
		[InlineData(1, 1, "1", "0")]
		public void TryReadNexKey(int start, int count, string expectedKeys, string expectedPositions) {
			var data = new SortedArrayData<int, int>(My.Array(start, count), x => x);
			var keys = new List<int>();
			var positions = new List<int>();
			while (data.TryReadNexKey(out var key, out var position)) {
				keys.Add(key);
				positions.Add(((IntPosition)position).Value);
			}
			string.Join(",", keys).Should().BeEquivalentTo(expectedKeys);
			string.Join(",", positions).Should().BeEquivalentTo(expectedPositions);
		}

		[Theory]
		[InlineData(0, 10, 0, "0,1,2,3,4,5,6,7,8,9")]
		[InlineData(0, 10, 2, "2,3,4,5,6,7,8,9")]
		[InlineData(0, 0, 0, "")]
		[InlineData(0, 0, 1, "")]
		public void ReadRemainData(int start, int count, int readCount, string expected) {
			var data = new SortedArrayData<int, int>(My.Array(start, count), x => x);
			for (int i = 0; i < readCount; i++) {
				data.TryReadNexKey(out _, out _);
			}
			var remaining = data.ReadRemaining();
			string.Join(",", remaining).Should().BeEquivalentTo(expected);
		}

		[Theory]
		[InlineData(0, 0, 0, 0, "")]
		[InlineData(0, 1, 1, 1, "1,0")]
		public void Prepend(int start, int count, int start2, int count2, string expected) {
			var data = new SortedArrayData<int, int>(My.Array(start, count), x => x);
			var data2 = new SortedArrayData<int, int>(My.Array(start2, count2), x => x);
			data.Prepend(data2);
			string.Join(",", data.Items).Should().BeEquivalentTo(expected);
		}

		[Theory]
		[InlineData(0, 0, 0, 0, "")]
		[InlineData(0, 1, 1, 1, "0,1")]
		public void Append(int start, int count, int start2, int count2, string expected) {
			var data = new SortedArrayData<int, int>(My.Array(start, count), x => x);
			data.Seek(new EndPosition());
			var data2 = new SortedArrayData<int, int>(My.Array(start2, count2), x => x);
			data.Append(data2);
			string.Join(",", data.Items).Should().BeEquivalentTo(expected);
		}
	}
}