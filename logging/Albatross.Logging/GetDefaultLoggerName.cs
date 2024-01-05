using System;
using System.Linq;

namespace Albatross.Logging {
	public interface IGetLoggerName {
		string Get(Type type);
	}

	public class GetDefaultLoggerName : IGetLoggerName {
		public string Get(Type type) => type.FullName;
	}

	public class GetShortenedLoggerNameByNamespacePrefix : IGetLoggerName {
		private readonly string[] namespacePrefixes;
		private readonly bool exclusive;

		public GetShortenedLoggerNameByNamespacePrefix(bool exclusive, string[] namespacePrefixes) {
			this.exclusive = exclusive;
			this.namespacePrefixes = namespacePrefixes;
		}

		public string Get(Type type) {
			if (namespacePrefixes.Any(x => type.FullName.StartsWith(x))) {
				return exclusive ? type.FullName : type.Name;
			} else {
				return exclusive ? type.Name: type.FullName;
			}
		}
	}
}
