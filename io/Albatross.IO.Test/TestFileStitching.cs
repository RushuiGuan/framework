using Albatross.Hosting.Test;
using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Albatross.Text;

namespace Albatross.IO.Test {
	public class TestFileStitching{
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
		public async Task TestTheStiching(string current, string changes, string expected) {
			var options = new FileStitchingOptions<int, int>(x => x.ToString(), x => x, x => int.Parse(x));
			var current_file = CreateTestFile(current);
			var changes_list = changes.IntArray().ToList();
			await new FileInfo(current_file).Stitch(changes_list, options);
			CombineLines(current_file).Should().BeEquivalentTo(string.Join(",", expected.IntArray()));
		}
		string CreateTestFile(string text) {
			var file = Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				foreach (var item in text.IntArray()) {
					writer.WriteLine(item);
				}
			}
			return file;
		}
		string CombineLines(string fileName) {
			var writer = new StringWriter();
			writer.WriteItems(File.ReadAllLines(fileName), ",");
			return writer.ToString();
		}
	}
}
