using Albatross.Testing;
using Albatross.Text.Table;
using System.Linq;
using Xunit;

namespace Albatross.Text.Test {
	public class TestStringTableColumnAdjustment {
		[Theory]
		[InlineData("a", "b", "c")]
		[InlineData("a", "b")]
		[InlineData("ab")]
		[InlineData("a")]
		public void TestColumnWidthAdjustment(params string[] columns) {
			var table = new StringTable(columns);
			Assert.Equal(columns.Sum(x => x.Length + 1) - 1, table.TotalWidth);
		}


		[Theory]
		[InlineData("")]
		[InlineData("a")]
		[InlineData("a", "AA", "BBB")]
		[InlineData("aaaa", "AA", "BBB")]
		public void TestColumnMaxWidthSetting(string column, params string[] data) {
			var table = new StringTable([column]);
			foreach (var row in data) {
				table.Add([row]);
			}
			Assert.Equal(data.Union([column]).Max(x => x.Length), table.Columns[0].MaxWidth);
		}


		[Theory]
		[InlineData("", "", 20, "")]
		[InlineData("1", "999", 0, "0")]
		[InlineData("10, 10, 10", "99,99,99", 0, "0,0,0")]
		[InlineData("10, 10, 10", "99,99,99", 10, "10,0,0")]
		[InlineData("10, 10, 10", "3,3,3", 10, "3,3,2")]
		[InlineData("10, 10, 10", "0,10,10", 10, "0,10,0")]
		[InlineData("10, 10, 10", "0,5,5", 15, "3,5,5")]
		[InlineData("10, 10, 10", "5,0,5", 15, "9,0,5")]
		public void TestColumnDisplayWidthCalculation(string maxWidth, string minWidth, int limit, string expectedDisplayWidth) {
			var maxWidthArray = maxWidth.IntArray();
			var minWidthArray = minWidth.IntArray();
			var table = new StringTable(maxWidthArray.Select(x=>x.ToString()));
			for (var i = 0; i < table.Columns.Length; i++) {
				table.Columns[i].SetMaxWidth(maxWidthArray[i]);
				table.Columns[i].MinWidth = minWidthArray[i];
			}
			table.AdjustColumnWidth(limit);
			Assert.True(table.TotalWidth <= limit);
			Assert.Equal(expectedDisplayWidth, string.Join(",", table.Columns.Select(x => x.DisplayWidth)));
		}
	}
}
