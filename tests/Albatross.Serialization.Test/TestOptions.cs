using Albatross.Reflection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Albatross.Serialization.Test {
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum DatasetType {
		Dataset,
		Secondary,
	}
	public record class Context {
		[JsonPropertyName("@base")]
		public string Base { get; init; }
		[JsonPropertyName("@vocab")]
		public string Vocab { get; init; }

		public Context(string @base, string vocab) {
			Base = @base;
			Vocab = vocab;
		}
	}
	public record class Dataset {
		[JsonPropertyName("@context")]
		public Context Context { get; init; } = null!;
		public string? Description { get; init; }
		public string Identifier { get; init; }
		public string Title { get; init; }
		public DateTime? Issued { get; init; }
		public DateTime? Modified { get; init; }

		public Dataset(string identifier, string title) {
			Identifier = identifier;
			Title = title;
		}
		[JsonPropertyName("@type")]
		public DatasetType Type { get; init; }
		public string? EarliestDelivery { get; init; }
		public string? LatestDelivery { get; init; }
		public string? PrimaryKey { get; init; }
		public string? Request { get; init; }
	}

	public class TestOptions {
		[Fact]
		public void TestDefaultOptions() {
			var option = new JsonSerializerOptions() {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};
			var text = GetType().GetEmbeddedFile("original.json");
			var value = JsonSerializer.Deserialize<Dataset>(text, option);
			Assert.Equal(DatasetType.Dataset, value?.Type);
			Assert.Equal("fields", value?.Identifier);
		}

		[Fact]
		public void TestModifiedSource() {
			var option = new JsonSerializerOptions() {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};
			var text = GetType().GetEmbeddedFile("modified.json");
			var value = JsonSerializer.Deserialize<Dataset>(text, option);
			Assert.Equal(DatasetType.Dataset, value?.Type);
			Assert.Equal("fields", value?.Identifier);
		}
	}
}
