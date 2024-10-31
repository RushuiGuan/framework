using Reqnroll.BoDi;
using Reqnroll.Infrastructure;
using System;
using System.Reflection;

namespace Albatross.ReqnrollPlugin {
	public interface ITestHostFinder {
		ReqnrollHost GetHost();
	}
	public class TestHostFinder : ITestHostFinder {
		private readonly ITestAssemblyProvider testAssemblyProvider;
		private readonly IObjectContainer rootBodiContainer;

		public TestHostFinder(ITestAssemblyProvider testAssemblyProvider, IObjectContainer rootBodiContainer) {
			this.testAssemblyProvider = testAssemblyProvider;
			this.rootBodiContainer = rootBodiContainer;
		}
		public ReqnrollHost GetHost() {
			var assembly = this.testAssemblyProvider.TestAssembly;
			foreach (var type in assembly.GetTypes()) {
				if (type.GetCustomAttribute<ReqnrollHostAttribute>() != null) {
					return (ReqnrollHost)Activator.CreateInstance(type, assembly, rootBodiContainer);
				}
			}
			throw new InvalidOperationException($"ReqnrollHost not found in assembly {assembly.GetName().Name}");
		}
	}
}