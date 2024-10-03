using Albatross.SpecFlowPlugin;
using BoDi;
using System.Reflection;

namespace Sample.Spec {

	[SpecFlowHost]
	public class MySpecFlowHost : SpecFlowHost {
		public MySpecFlowHost(Assembly testAssembly, IObjectContainer rootBoDiContainer) : base(testAssembly, rootBoDiContainer) {
		}
	}
}
