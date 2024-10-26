using System;

namespace Albatross.Caching.Test.CacheKeys {
	public record class MyData {
		public MyData(string value) {
			Value = value;
		}
		public MyData() {
			Value = Guid.NewGuid().ToString();
		}

		public string Value { get; set; }
	}
}