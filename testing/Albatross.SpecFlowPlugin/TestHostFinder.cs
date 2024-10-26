using BoDi;
using System;
using System.Reflection;
using TechTalk.SpecFlow.Infrastructure;

namespace Albatross.SpecFlowPlugin {
	public interface ITestHostFinder {
		SpecFlowHost GetHost();
	}
	public class TestHostFinder : ITestHostFinder {
		private readonly ITestAssemblyProvider testAssemblyProvider;
		private readonly IObjectContainer rootBodiContainer;

		public TestHostFinder(ITestAssemblyProvider testAssemblyProvider, IObjectContainer rootBodiContainer) {
			this.testAssemblyProvider = testAssemblyProvider;
			this.rootBodiContainer = rootBodiContainer;
		}
		public SpecFlowHost GetHost() {
			var assembly = this.testAssemblyProvider.TestAssembly;
			foreach (var type in assembly.GetTypes()) {
				if (type.GetCustomAttribute<SpecFlowHostAttribute>() != null) {
					return (SpecFlowHost)Activator.CreateInstance(type, assembly, rootBodiContainer);
				}
			}
			throw new InvalidOperationException($"SpecFlowHost not found in assembly {assembly.GetName().Name}");
		}
	}
}