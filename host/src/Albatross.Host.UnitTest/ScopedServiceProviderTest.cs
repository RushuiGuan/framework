using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Host.UnitTest {
    [TestFixture]
    public class ScopedServiceProviderTest {
        public interface IInterface1 { }
        public interface IInterface2 { }
        public class Test1 : IInterface1, IInterface2 {
        }

        public class Test2 : IInterface1 {
        }
        public class Test3 : IInterface1 {
        }
        public class Test4 : IInterface1 {
        }

        [Test]
        public void MultipleInterfaceOfTheSameClass() {
            ServiceCollection svc = new ServiceCollection();
            svc.AddSingleton<Test1>();
            svc.AddSingleton<IInterface1>(p => p.GetRequiredService<Test1>());
            svc.AddSingleton<IInterface2>(p => p.GetRequiredService<Test1>());
            var provider = svc.BuildServiceProvider();
            var obj1 = provider.GetRequiredService<IInterface1>();
            var obj2 = provider.GetRequiredService<IInterface2>();
            Assert.AreSame(obj1, obj2);
        }

        [Test]
        public void CollectionRegistration() {
            ServiceCollection svc = new ServiceCollection();
            svc.AddTransient<IInterface1, Test1>();
            svc.AddTransient<IInterface1, Test2>();
            svc.AddTransient<IInterface1, Test3>();
            svc.AddTransient<IInterface1, Test4>();

            var provider = svc.BuildServiceProvider();
            var items = provider.GetRequiredService<IEnumerable<IInterface1>>().ToArray();
            Assert.AreEqual(4, items.Length);
            Assert.True(items[0] is Test1);
            Assert.True(items[1] is Test2);
            Assert.True(items[2] is Test3);
            Assert.True(items[3] is Test4);

        }
    }
}
