using Albatross.Reqnroll;

namespace Sample.Reqnroll.StepDefinitions {
	[Binding]
	public class MySharedBinding : SharedBindings {
		public MySharedBinding(ScenarioContext scenario) : base(scenario) {
		}
	}
}
