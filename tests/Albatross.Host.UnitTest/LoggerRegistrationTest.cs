using NUnit.Framework;
using System;

namespace Albatross.Host.UnitTest {
    [TestFixture]
	public class LoggerRegistrationTest {
		public interface ITest {
			int Run();
			Type GetSourceType();
		}

		public interface ITest<T> : ITest { }

		public interface ITestFactory {
			ITest<T> Create<T>();
		}

		public class Test1<T> : ITest<T> {
			public int Run() {
				return 100;
			}
			public Type GetSourceType() {
				return typeof(T);
			}
		}
		public class TestFactory : ITestFactory {
			public ITest<T> Create<T>() {
				return new Test1<T>();
			}
		}
		public class Consumer {
			public ITest Test { get; private set; }
			public Consumer(ITest test) {
				this.Test = test;
			}
		}
	}
}
