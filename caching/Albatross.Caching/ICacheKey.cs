namespace Albatross.Caching {
	/// <summary>
	/// A cache key is made of the prefix section and the value section.  Each section of the key always ends with ':'.  
	/// The prefix section has a fix text and the value section can change or be empty.  When the value is empty, the value 
	/// section would be omitted.
	/// For example: mk:123: would be a key with prefix "mk" and value "123", mk: would be a key with prefix "mk" and no value.
	/// </summary>
	public interface ICacheKey {
		public const string Delimiter = ":";
		public const char Asterisk = '*';
		/// <summary>
		/// the first part of the cache key that will not change
		/// </summary>
		string Prefix { get; }

		/// <summary>
		/// the complete cache key
		/// </summary>
		string Key { get; }

		/// <summary>
		/// The wildcard key that matches itself and its children
		/// </summary>
		string WildCardKey { get; }

		/// <summary>
		/// the key that will match all values of the same prefix
		/// </summary>
		string ResetKey { get; }
	}
	public class CacheKey : ICacheKey {
		public const string DefaultPrefix = "df";
		public ICacheKey? Parent { get; }
		public string Prefix { get; }
		public string Key { get; }
		public string WildCardKey { get; }
		public string ResetKey { get; }
		public override string ToString() => Key;

		public CacheKey(ICacheKey? parent, string prefix, string? value) {
			this.Parent = parent;
			this.Prefix = string.IsNullOrEmpty(prefix) ? DefaultPrefix : prefix;

			Key = $"{parent?.Key}{Prefix}{ICacheKey.Delimiter}";
			if (!string.IsNullOrEmpty(value)) {
				Key = $"{Key}{value}{ICacheKey.Delimiter}";
			}
			WildCardKey = $"{Key}{ICacheKey.Asterisk}";
			ResetKey = $"{parent?.Key}{Prefix}{ICacheKey.Delimiter}{ICacheKey.Asterisk}";
		}
		public CacheKey(string prefix, string? value) : this(null, prefix, value) { }
		public CacheKey(string? value) : this(null, DefaultPrefix, value) { }
	}
}
