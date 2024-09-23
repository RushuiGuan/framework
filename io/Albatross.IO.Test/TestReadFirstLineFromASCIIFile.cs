using FluentAssertions;
using System.IO;
using Xunit;

namespace Albatross.IO.Test {
	public class TestReadFirstLineFromASCIIFile {
		[Fact]
		public void EmptyFile() {
			var file = Path.GetTempFileName();
			using (var stream = File.OpenRead(file)) {
				var result = stream.TryReadLine(out var line, out _);
				result.Should().BeFalse();
				line.Should().BeNull();
			}
		}

		[Fact]
		public void SingleLineWithNoReturn() {
			const string text = "hello";
			var file = Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				writer.Write(text);
			}

			using (var stream = File.OpenRead(file)) {
				var result = stream.TryReadLine(out var line, out _);
				result.Should().BeTrue();
				line.Should().Be(text);
			}
		}

		[Fact]
		public void SingleLineWithReturn() {
			const string text = "hello";
			var file = Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				writer.WriteLine(text);
			}

			using (var stream = File.OpenRead(file)) {
				var result = stream.TryReadLine(out var line, out _);
				result.Should().BeTrue();
				line.Should().Be(text);
			}
		}

		[Fact]
		public void MultiLine() {
			const string line1 = "line1";
			const string line2 = "line2";
			var file = Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				writer.WriteLine(line1);
				writer.Write(line2);
			}

			using (var stream = File.OpenRead(file)) {
				var result = stream.TryReadLine(out var line, out _);
				result.Should().BeTrue();
				line.Should().Be(line1);

				result = stream.TryReadLine(out line, out _);
				result.Should().BeTrue();
				line.Should().Be(line2);

				result = stream.TryReadLine(out line, out _);
				result.Should().BeFalse();
				line.Should().BeNull();
			}
		}

		[Fact]
		public void MultiLineThatEndsWithLineFeed() {
			const string line1 = "line1";
			const string line2 = "line2";
			var file = Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				writer.WriteLine(line1);
				writer.WriteLine(line2);
			}

			using (var stream = File.OpenRead(file)) {
				var result = stream.TryReadLine(out var line, out _);
				result.Should().BeTrue();
				line.Should().Be(line1);

				result = stream.TryReadLine(out line, out _);
				result.Should().BeTrue();
				line.Should().Be(line2);

				result = stream.TryReadLine(out line, out _);
				result.Should().BeFalse();
				line.Should().BeNull();
			}
		}

		[Fact]
		public void MultiLineThatEndsWithLineFeedUnixStyle() {
			const string line1 = "line1";
			const string line2 = "line2";
			var file = Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				writer.Write(line1);
				writer.Write('\n');
				writer.Write(line2);
				writer.Write('\n');
			}

			using (var stream = File.OpenRead(file)) {
				var result = stream.TryReadLine(out var line, out _);
				result.Should().BeTrue();
				line.Should().Be(line1);

				result = stream.TryReadLine(out line, out _);
				result.Should().BeTrue();
				line.Should().Be(line2);

				result = stream.TryReadLine(out line, out _);
				result.Should().BeFalse();
				line.Should().BeNull();
			}
		}

		[Fact]
		public void MultiLineWithMixedEmptyLineAndText() {
			const string line1 = "line1";
			const string line2 = "line2";
			var file = Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				writer.WriteLine(line1);
				writer.WriteLine();
				writer.WriteLine(line2);
			}

			using (var stream = File.OpenRead(file)) {
				var result = stream.TryReadLine(out var line, out _);
				result.Should().BeTrue();
				line.Should().Be(line1);

				result = stream.TryReadLine(out line, out _);
				result.Should().BeTrue();
				line.Should().Be(line2);

				result = stream.TryReadLine(out line, out _);
				result.Should().BeFalse();
				line.Should().BeNull();
			}
		}

		[Fact]
		public void MultiLineWithMixedEmptyLine_EmptySpace_Text() {
			const string line1 = "line1";
			const string line2 = "line2";
			const string space = "     ";
			var file = Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				writer.WriteLine(line1);
				writer.WriteLine();
				writer.WriteLine(space);
				writer.WriteLine();
				writer.WriteLine(line2);
			}

			using (var stream = File.OpenRead(file)) {
				var result = stream.TryReadLine(out var line, out _);
				result.Should().BeTrue();
				line.Should().Be(line1);

				result = stream.TryReadLine(out line, out _);
				result.Should().BeTrue();
				line.Should().Be(space);

				result = stream.TryReadLine(out line, out _);
				result.Should().BeTrue();
				line.Should().Be(line2);

				result = stream.TryReadLine(out line, out _);
				result.Should().BeFalse();
				line.Should().BeNull();
			}
		}
	}
}
