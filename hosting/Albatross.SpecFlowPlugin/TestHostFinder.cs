using System;
using System.Reflection;
using TechTalk.SpecFlow.Infrastructure;

namespace Albatross.SpecFlowPlugin {
	public interface ITestHostFinder {
		SpecFlowHost GetHost();
	}
	public class TestHostFinder : ITestHostFinder {
		private readonly ITestAssemblyProvider testAssemblyProvider;

		public TestHostFinder(ITestAssemblyProvider testAssemblyProvider) {
			this.testAssemblyProvider = testAssemblyProvider;
		}
		public SpecFlowHost GetHost() {
			var assembly = this.testAssemblyProvider.TestAssembly;
			foreach (var type in assembly.GetTypes()) {
				if (type.GetCustomAttribute<SpecFlowHostAttribute>() != null) {
					return (SpecFlowHost)Activator.CreateInstance(type, assembly);
				}
			}
			throw new InvalidOperationException($"SpecFlowHost not found in assembly {assembly.GetName().Name}");
		}
	}
}
