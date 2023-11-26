using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.Test.DI {
	public record class MyClass1 : IMyInterface1 {
		public Guid Id { get; }
		public MyClass1() {
			Id = Guid.NewGuid();
		}
	}
	public record class MyClass2 : IMyInterface1 {
		public Guid Id { get; }
		public MyClass2() {
			Id = Guid.NewGuid();
		}
	}
	public record class MyClass3 : IMyInterface1 {
		public Guid Id { get; }
		public MyClass3() {
			Id = Guid.NewGuid();
		}
	}
	public record class MyClass4 : IMyInterface1 {
		public Guid Id { get; }
		public MyClass4() {
			Id = Guid.NewGuid();
		}
	}
	public interface IMyInterface1 { }
	public class TestDependencyInjection {
		[Fact]
		public void TestDoubleRegistration_TheIncorrectWay() {
			var services = new ServiceCollection();
			services.AddSingleton<MyClass1>();
			services.AddSingleton<IMyInterface1, MyClass1>();
			var provider = services.BuildServiceProvider();
			var item1 = provider.GetRequiredService<MyClass1>();
			var item2 = provider.GetRequiredService<IMyInterface1>();
			Assert.NotSame(item1, item2);
		}

		[Fact]
		public void TestDoubleRegistration_TheCorrectWay() {
			var services = new ServiceCollection();
			services.AddSingleton<MyClass1>();
			services.AddSingleton<IMyInterface1>(provider => provider.GetRequiredService<MyClass1>());
			var provider = services.BuildServiceProvider();
			var item1 = provider.GetRequiredService<MyClass1>();
			var item2 = provider.GetRequiredService<IMyInterface1>();
			Assert.Same(item1, item2);
		}

		[Fact]
		public void TestDoubleRegistration_TheSecondCorrectAndConfusingWay() {
			var services = new ServiceCollection();
			services.AddSingleton(typeof(MyClass1));
			services.AddSingleton(typeof(IMyInterface1), x=> (IMyInterface1)x.GetRequiredService(typeof(MyClass1)));
			var provider = services.BuildServiceProvider();
			var item1 = provider.GetRequiredService<MyClass1>();
			var item2 = provider.GetRequiredService<IMyInterface1>();
			Assert.Same(item1, item2);
		}

		[Fact]
		public void TestEnumerationRegistration_TheGenericWay() {
			var services = new ServiceCollection();
			services.TryAddEnumerable(ServiceDescriptor.Singleton<IMyInterface1, MyClass1>());
			services.TryAddEnumerable(ServiceDescriptor.Singleton<IMyInterface1, MyClass2>());
			services.TryAddEnumerable(ServiceDescriptor.Singleton<IMyInterface1, MyClass3>());
			services.TryAddEnumerable(ServiceDescriptor.Singleton<IMyInterface1, MyClass4>());
			var provider = services.BuildServiceProvider();
			var items = provider.GetRequiredService<IEnumerable<IMyInterface1>>();
			Assert.Collection(items,
				x => Assert.True(x is MyClass1),
				x => Assert.True(x is MyClass2),
				x => Assert.True(x is MyClass3),
				x => Assert.True(x is MyClass4));
		}
		[Fact]
		public void TestEnumerationRegistration_TheTypeWay() {
			var services = new ServiceCollection();
			services.TryAddSingleton(typeof(MyClass1));
			services.TryAddSingleton(typeof(MyClass2));
			services.TryAddSingleton(typeof(MyClass3));
			services.TryAddSingleton(typeof(MyClass4));

			services.AddSingleton<IMyInterface1, MyClass1>(x => x.GetRequiredService<MyClass1>());
			services.AddSingleton<IMyInterface1, MyClass2>(x => x.GetRequiredService<MyClass2>());
			services.AddSingleton<IMyInterface1, MyClass3>(x => x.GetRequiredService<MyClass3>());
			services.AddSingleton<IMyInterface1, MyClass4>(x => x.GetRequiredService<MyClass4>());

			var provider = services.BuildServiceProvider();
			var items = provider.GetRequiredService<IEnumerable<IMyInterface1>>();
			Assert.Collection(items,
				x => Assert.True(x is MyClass1),
				x => Assert.True(x is MyClass2),
				x => Assert.True(x is MyClass3),
				x => Assert.True(x is MyClass4));
		}

	}
}
