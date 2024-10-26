using System;

namespace Albatross.EFCore {
	public class MissingNavigationPropertyException : Exception {
		public MissingNavigationPropertyException(params string[] names) : base($"Navigation property {string.Join('.', names)} is not loaded") {
		}
	}
}