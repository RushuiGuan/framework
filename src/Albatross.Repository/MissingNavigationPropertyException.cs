using System;

namespace Albatross.Repository {
	public class MissingNavigationPropertyException : Exception {
		public MissingNavigationPropertyException(params string[] names) : base($"Navigation property {string.Join('.', names)} is not loaded") {
		}
	}
}
