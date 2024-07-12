namespace Albatross.Caching.Test.CacheKeys {
	public class CustomKey1: CacheKey{
		public CustomKey1(string? value) : base("custom-key", value, true) {
		}
	}
	public class CustomKey2: CacheKey{
		public CustomKey2(string? value) : base("custom-key2", value, true) {
		}
	}
}
