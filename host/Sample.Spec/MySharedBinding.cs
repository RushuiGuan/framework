using Albatross.SpecFlowPlugin;

namespace Sample.Spec {
	[Binding]
	public class MySharedBinding : ArgumentTransformations {
		public MySharedBinding(ScenarioContext scenario) : base(scenario) {
		}
	}
}
