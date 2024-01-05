using System;
using System.Linq;

namespace Albatross.Logging {
	public interface IGetLoggerName {
		string Get(Type type);
	}
	public class GetLoggerName : IGetLoggerName {
		public string Get(Type type) => type.FullName;
	}
	public class GetLoggerNameByNamespacePrefixExclusion : IGetLoggerName {
		private readonly string[] excludeNamespaces;
		public GetLoggerNameByNamespacePrefixExclusion(string[] excludeNamespaces) {
			this.excludeNamespaces = excludeNamespaces;
		}
		public string Get(Type type) {
			if (excludeNamespaces.Any(x => type.FullName.StartsWith(x))) {
				return type.FullName;
			} else {
				return type.Name;
			}
		}
	}
}
