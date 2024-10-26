using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Spec.StepDefinitions {
	[Binding]
	public class MySharedBindings : Albatross.SpecFlow.ArgumentTransformations {
		public MySharedBindings(ScenarioContext scenario) : base(scenario) {
		}
	}
}