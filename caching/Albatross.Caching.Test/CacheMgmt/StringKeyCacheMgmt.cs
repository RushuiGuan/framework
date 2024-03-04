using System;

namespace Albatross.Caching.Test.CacheMgmt {
	public class MyKey : CacheKey {
		public MyKey(string value) : base(null, "my-key", value) {
		}
	}
	public record class MyData {
		public MyData(string value) {
			Value = value;
		}
		public MyData() {
			Value = Guid.NewGuid().ToString();
		}

		public string Value { get; set; }
	}
	public class StringKey: CacheKey{
		public StringKey(string? value) : base("string-key", value) {
		}
	}
	public class StringKey2: CacheKey{
		public StringKey2(string? value) : base("string-key-2", value) {
		}
	}
}
