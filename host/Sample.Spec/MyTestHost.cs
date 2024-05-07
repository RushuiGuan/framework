using Albatross.SpecFlowPlugin;
using BoDi;
using System.Reflection;

namespace Sample.Spec {

	[SpecFlowHost]
	public class MyTestHost : SpecFlowHost {
		public MyTestHost(Assembly testAssembly, IObjectContainer rootBoDiContainer) : base(testAssembly, rootBoDiContainer) {
		}
	}
}
