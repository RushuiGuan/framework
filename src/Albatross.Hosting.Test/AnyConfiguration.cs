using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Hosting.Test {
	public class EmptyConfigurationSection : IConfigurationSection {
		public string? this[string key] {
			get => null;
			set => throw new NotSupportedException();
		}
		public string Key => string.Empty;
		public string Path => string.Empty;
		public string? Value { get => null; set => throw new NotSupportedException(); }

		public IEnumerable<IConfigurationSection> GetChildren() => Enumerable.Empty<IConfigurationSection>();

		public IChangeToken GetReloadToken() => throw new NotSupportedException();
		public IConfigurationSection GetSection(string key) => this;
	}

	public class AnyConfigurationSection : AnyConfiguration, IConfigurationSection {
		string? _value;
		public AnyConfigurationSection(string parent, string path, string? value) {
			var keys = ParsePath(path);
			this.Key = keys.key;
			this.Path = string.IsNullOrEmpty(parent) ? keys.key : $"{parent}{ConfingNameDelimiter}{keys.key}";
			if (string.IsNullOrEmpty(keys.childPath)) {
				this._value = value ?? Any;
			} else {
				var child = new AnyConfigurationSection(this.Path, keys.childPath, value);
				this.children[child.Key] = child;
				this._value = null;
			}
		}
		public string? Value {
			get => this.children.Count > 0 ? null : _value;
			set => _value = value;
		}
		public string Key { get; init; }
		public override IConfigurationSection GetSection(string key) {
			return base.GetSection(key);
		}
	}
	/// <summary>
	///  this configuration class will return "any" for any request, unless the requested section has been set with a value first
	/// </summary>
	public class AnyConfiguration : IConfiguration {
		public const char ConfingNameDelimiter = ':';
		public const string Any = "any";
		public string Path { get; init; }
		protected Dictionary<string, IConfigurationSection> children = new Dictionary<string, IConfigurationSection>();

		public AnyConfiguration() {
			this.Path = string.Empty;
		}

		public string? this[string key] {
			get => GetSection(key).Value;
			set {
				var section = GetSection(key);
				section.Value = value;
			}
		}
		public IEnumerable<IConfigurationSection> GetChildren() => children.Values;
		public IChangeToken GetReloadToken() => throw new NotSupportedException();
		public virtual IConfigurationSection GetSection(string key) {
			if (string.IsNullOrEmpty(key)) {
				return new EmptyConfigurationSection();
			}
			var keys = ParsePath(key);
			if (!this.children.TryGetValue(keys.key, out var section)) {
				section = new AnyConfigurationSection(this.Path, key, null);
				this.children[section.Key] = section;
			}
			if (string.IsNullOrEmpty(keys.childPath)) {
				return section;
			} else {
				return section.GetSection(keys.childPath);
			}
		}
		public (string key, string? childPath) ParsePath(string path) {
			if (string.IsNullOrWhiteSpace(path)) {
				throw new ArgumentException($"Invalid configuration path: {path}");
			} else {
				var index = path.IndexOf(ConfingNameDelimiter);
				if (index == -1) {
					return (path, null);
				} else {
					return (path.Substring(0, index), path.Substring(index + 1));
				}
			}
		}
	}
}