﻿using System.Text.Json;
using Xunit;

#nullable enable
namespace Albatross.Test.Serialization {
	public class JobDto {
		public JobDto(string name) {
			Name = name;
		}
		public string Name { get; private set; }
	}

	public class TestNullable {
		[Fact]
		public void TestSerialization() {
			var dto = new JobDto("test");
			var text = JsonSerializer.Serialize(dto);
			Assert.NotNull(text);
		}
		[Fact]
		public void TestDeserialization() {
			string text = "{\"Name\": \"test\"}";
			var dto = JsonSerializer.Deserialize<JobDto>(text);
			Assert.NotNull(dto);
		}
	}
}
#nullable restore

