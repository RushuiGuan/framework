using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Hosting.Test {
	public class TestScope : IDisposable {
		public IServiceScope scope { get; }

		public TestScope(IServiceScope scope) {
			this.scope = scope;
		}

		public void Dispose() {
			this.scope.Dispose();
		}

		public T Get<T>() where T : notnull {
			return scope.ServiceProvider.GetRequiredService<T>();
		}
		public IServiceProvider Provider => scope.ServiceProvider;
	}
}
