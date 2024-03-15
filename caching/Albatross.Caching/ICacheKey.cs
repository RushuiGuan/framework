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

		public bool HasChildren { get; }
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

		public static string BuildKey(string? parent, string prefix, string? value) {
			string key = $"{parent}{prefix}{Delimiter}";
			if (!string.IsNullOrEmpty(value)) {
				key = $"{key}{value}{Delimiter}";
			}
			return key;
		}
		public static string BuildWildCardKey(string key) {
			return $"{key}{Asterisk}";
		}
		public static string BuildResetKey(ICacheKey? parent, string prefix) {
			return $"{parent?.Key}{prefix}{ICacheKey.Delimiter}{ICacheKey.Asterisk}";
		}
	}
	public class CacheKey : ICacheKey {
		public ICacheKey? Parent { get; }
		public string Prefix { get; }
		public string Key { get; }
		public string WildCardKey { get; }
		public string ResetKey { get; }

		public bool HasChildren { get; }

		public override string ToString() => Key;
		public const string DefaultPrefix = "df";

		public CacheKey(ICacheKey? parent, string prefix, string? value, bool hasChildren) {
			if (parent != null && !parent.HasChildren) {
				throw new System.ArgumentException("parent key must have children");
			}
			this.Parent = parent;
			this.Prefix = string.IsNullOrEmpty(prefix) ? DefaultPrefix : prefix;
			Key = ICacheKey.BuildKey(parent?.Key, Prefix, value);
			WildCardKey = ICacheKey.BuildWildCardKey(Key);
			ResetKey = ICacheKey.BuildResetKey(parent, Prefix);
			HasChildren = hasChildren;
		}
		public CacheKey(string prefix, string? value, bool hasChildren) : this(null, prefix, value, hasChildren) { }
		public CacheKey(string? value, bool hasChildren) : this(null, DefaultPrefix, value, hasChildren) { }
	}
}
