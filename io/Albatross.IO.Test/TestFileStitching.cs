using Albatross.Hosting.Test;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System;
using MessagePack;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using System.Threading;

namespace Albatross.IO.Test {
	[MessagePackObject]
	public record class TestData {
		public TestData() { }

		public TestData(string text) {
			var parts = text.Split(',');
			Id = int.Parse(parts[0]);
			Value = Guid.Parse(parts[1]);
		}


		[Key(0)]
		public int Id { get; set; }
		[Key(1)]
		public Guid Value { get; set; }

		public override string ToString() {
			return $"{Id},{Value}";
		}
	}


	public class TestTextFileStitching {
		[Theory]
		[InlineData("", "", "")]
		[InlineData("", "1-5", "1-5")]
		[InlineData("1-5", "", "1-5")]
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
		public async Task TestTextStiching(string current_text, string changes_text, string expected_text) {
			var current = CreateTestData(current_text).ToArray();
			var changes = CreateTestData(changes_text).ToArray();
			var lookup = new Dictionary<int, TestData>();
			foreach (var item in current) {
				lookup[item.Id] = item;
			}
			foreach (var item in changes) {
				lookup[item.Id] = item;
			}

			var current_file = await SaveDataAsText(current, null);
			var options = new TextFileStitchingOptions<int, TestData>(current_file, x => x.ToString(), x => x.Id, x => new TestData(x).Id);
			var logger = new Mock<ILogger>().Object;
			await options.StitchText(changes, logger);
			var result = ReadDataFromTextFile(options.File.FullName).ToArray();

			// first check the keys
			result.Select(x => x.Id).ToArray().Should().BeEquivalentTo(expected_text.IntArray());
			// now check each value
			foreach (var item in result) {
				item.Value.Should().Be(lookup[item.Id].Value);
			}
			// now check the index positions in reverse
			var indexes = await options.IndexFilename.ReadFileIndex<int>();
			for (int i = indexes.Length - 1; i >= 0; i--) {
				var index = indexes[i];
				using (var stream = File.OpenRead(options.File.FullName)) {
					stream.Seek(index.Position, SeekOrigin.Begin);
					var line = await new StreamReader(stream).ReadLineAsync();
					Assert.NotNull(line);
					var item = new TestData(line);
					item.Should().BeEquivalentTo(lookup[index.Key]);
				}
			}
		}


		[Theory]
		[InlineData("", "", "")]
		[InlineData("", "1-5", "1-5")]
		[InlineData("1-5", "", "1-5")]
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
		public async Task TestBinaryStitching(string current_text, string changes_text, string expected_text) {
			var current = CreateTestData(current_text).ToArray();
			var changes = CreateTestData(changes_text).ToArray();
			var lookup = new Dictionary<int, TestData>();
			foreach (var item in current) {
				lookup[item.Id] = item;
			}
			foreach (var item in changes) {
				lookup[item.Id] = item;
			}

			var current_file = await SaveDataAsBinary(current, null);
			var options = new FileStitchingOptions<int, TestData>(current_file, x => x.Id);
			var logger = new Mock<ILogger>().Object;
			await options.StitchBinary(changes, logger);
			var result = (await ReadDataFromBinary(options.File.FullName)).ToArray();

			// first check the keys
			result.Select(x => x.Id).ToArray().Should().BeEquivalentTo(expected_text.IntArray());
			// now check each value
			foreach (var item in result) {
				item.Value.Should().Be(lookup[item.Id].Value);
			}
			// now check the index positions in reverse
			var indexes = await options.IndexFilename.ReadFileIndex<int>();
			for (int i = indexes.Length - 1; i >= 0; i--) {
				var index = indexes[i];
				using (var stream = File.OpenRead(options.File.FullName)) {
					stream.Seek(index.Position, SeekOrigin.Begin);
					var item = await MessagePackSerializer.DeserializeAsync<TestData>(stream, StitchExtensions.RecordSerializationOptions);
					item.Should().BeEquivalentTo(lookup[index.Key]);
				}
			}
		}
		IEnumerable<TestData> CreateTestData(string text) {
			foreach (var item in text.IntArray()) {
				yield return new TestData {
					Id = item,
					Value = Guid.NewGuid()
				};
			}
		}
		async Task<string> SaveDataAsText(IEnumerable<TestData> data, string? filename) {
			var file = filename ?? Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				foreach (var item in data) {
					await writer.WriteLineAsync(item.ToString());
				}
			}
			return file;
		}
		async Task<string> SaveDataAsBinary(IEnumerable<TestData> data, string? filename) {
			var file = filename ?? Path.GetTempFileName();
			using (var stream = File.OpenWrite(file)) {
				foreach (var item in data) {
					await MessagePackSerializer.SerializeAsync(stream, item, StitchExtensions.RecordSerializationOptions);
				}
				stream.SetLength(stream.Position);
			}
			return file;
		}
		IEnumerable<TestData> ReadDataFromTextFile(string file) {
			using (var reader = new StreamReader(file)) {
				while (!reader.EndOfStream) {
					var line = reader.ReadLine();
					if (!string.IsNullOrEmpty(line)) {
						yield return new TestData(line);
					}
				}
			}
		}
		async Task<IEnumerable<TestData>> ReadDataFromBinary(string file) {
			using (var stream = File.OpenRead(file)) {
				var items = stream.ReadAsStream<TestData>(StitchExtensions.RecordSerializationOptions);
				var list = new List<TestData>();
				await foreach (var record in items) {
					list.Add(record);
				}
				return list;
			}
		}
	}
}
