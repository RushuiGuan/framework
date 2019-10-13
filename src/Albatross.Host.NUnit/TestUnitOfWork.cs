using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Host.NUnit {
    public class TestUnitOfWork: IDisposable {
        IServiceScope scope;
        public TestUnitOfWork(IServiceScope scope) {
            this.scope = scope;
        }

        public void Dispose() {
            this.scope.Dispose();
        }

        public T Get<T>() {
            return scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
