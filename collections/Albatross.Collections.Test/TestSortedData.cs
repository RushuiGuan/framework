using Albatross.Testing;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Collections.Test {
	public class TestSortedData {
		[Theory]
		[InlineData(0, 0, 0, 0, "wrap")]
		[InlineData(0, 1, 1, 1, "wrap")]
		[InlineData(0, 1, 0, 1, "wrap")]
		[InlineData(0, 1, 0, 0, "wrap")]

		[InlineData(1, 1, 0, 2, "contain")]
		[InlineData(1, 2, 0, 3, "contain")]

		[InlineData(1, 1, 2, 2, "leftie")]
		[InlineData(0, 1, 2, 2, "leftie")]
		[InlineData(1, 1, 2, 3, "leftie")]
		[InlineData(0, 1, 2, 3, "leftie")]

		[InlineData(2, 2, 1, 1, "rightie")]
		[InlineData(2, 3, 1, 1, "rightie")]
		[InlineData(3, 3, 1, 2, "rightie")]
		[InlineData(2, 3, 0, 1, "rightie")]

		[InlineData(1, 2, 2, 4, "leftie-with-overlap")]
		[InlineData(2, 2, 2, 4, "leftie-with-overlap")]
		[InlineData(1, 3, 2, 4, "leftie-with-overlap")]
		[InlineData(2, 3, 2, 4, "leftie-with-overlap")]

		[InlineData(4, 4, 2, 4, "rightie-with-overlap")]
		[InlineData(3, 4, 2, 4, "rightie-with-overlap")]
		[InlineData(3, 5, 2, 4, "rightie-with-overlap")]
		[InlineData(4, 5, 2, 4, "rightie-with-overlap")]
		public void situation_test(int changes_firstKey, int changes_lastKey, int current_firstkey, int current_lastkey, string expected) {
			var current = new MySortedData(current_firstkey, current_lastkey);
			var changes = new MySortedData(changes_firstKey, changes_lastKey);
			if (current.Leftie(changes)) {
				"leftie".Should().Be(expected);
			}
			if (current.Rightie(changes)) {
				"rightie".Should().Be(expected);
			}
			if (current.WrappBy(changes)) {
				"wrap".Should().Be(expected);
			}
			if (current.Contains(changes)) {
				"contain".Should().Be(expected);
			}
			if (current.LeftieWithOverlap(changes)) {
				"leftie-with-overlap".Should().Be(expected);
			}
			if (current.RightieWithOverlap(changes)) {
				"rightie-with-overlap".Should().Be(expected);
			}
		}

		[Theory]
		//[InlineData("", "", "")]
		// contain
		[InlineData("1-o5", "4", "1,3,4,5")]
		[InlineData("1-o5", "2-e4", "1,2,4,5")]
		[InlineData("0-5", "1,4", "0,1,4,5")]
		// rightie
		[InlineData("0", "1", "0,1")]
		[InlineData("0-e2", "3-o5", "0,2,3,5")]
		// rightie-with-overlap
		[InlineData("0-1", "1", "0,1")]
		[InlineData("0-1", "1-2", "0,1,2")]
		[InlineData("0-5", "2,5", "0,1,2,5")]
		[InlineData("1-o5", "4-e6", "1,3,4,6")]
		// leftie
		[InlineData("1", "0", "0,1")]
		[InlineData("2-3", "0-1", "0,1,2,3")]
		// leftie-with-overlap
		[InlineData("0-5", "0-1", "0,1,2,3,4,5")]
		[InlineData("1-2", "0-1", "0,1,2")]
		[InlineData("1-2", "1", "1,2")]
		// wrap
		[InlineData("0-o5", "0-5", "0,1,2,3,4,5")]
		[InlineData("2-3", "0-e6", "0,2,4,6")]
		[InlineData("2-3", "2-3", "2,3")]
		[InlineData("2-3", "1-3", "1,2,3")]
		[InlineData("2-3", "2-4", "2,3,4")]
		public async Task TestIntArray(string current, string changes, string expected) {
			var current_data = new SortedArrayData<int, int>(current.IntArray(), x => x);
			var changes_data = new SortedArrayData<int, int>(changes.IntArray(), x => x);
			await current_data.Stitch(changes_data);
			var result = string.Join(',', current_data.Items);
			result.Should().Be(expected);
		}
	}
}