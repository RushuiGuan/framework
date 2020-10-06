using Albatross.Reflection;
using Albatross.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Albatross.Test.Serialization {
	public class TestDynamicConversion{
		[Fact]
		public void TestNormal() {
			string json = GetType().GetEmbeddedFile("DynamicConversion.json");
			JsonElement elem = JsonSerializer.Deserialize<JsonElement>(json);
			var value = elem.Convert();
			Assert.Equal("test", value.Name);
			Assert.Equal(1.1, value.onepointone);
			Assert.Equal(22, value.integer);
			Assert.Equal(new DateTime(2020,9,1), value.date);


			Assert.Equal("inner_name", value.inner.Name);
			Assert.Equal(1.2, value.inner.onepointtwo);
			Assert.Equal(33, value.inner.integer);
			Assert.Equal(new DateTime(2020, 9, 2), value.inner.date);
		}
	}
}
