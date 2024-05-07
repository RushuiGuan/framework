using Albatross.SpecFlowPlugin;
using System.Reflection;

namespace Sample.Spec {

	[SpecFlowHost]
	public class MyTestHost : SpecFlowHost {
		public MyTestHost(Assembly testAssembly) : base(testAssembly) {
		}
	}
}
