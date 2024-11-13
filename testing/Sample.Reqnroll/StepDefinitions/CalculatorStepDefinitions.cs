using Albatross.Reqnroll;
using AutoFixture;

namespace Sample.Reqnroll.StepDefinitions {
	public class MyClass {
		public string? Name { get; set; }
		public int Value { get; set; }
		public int? Age { get; set; }
	}
	[Binding]
	public sealed class CalculatorStepDefinitions {
		private readonly ScenarioContext scenario;
		private readonly Fixture fixture = new Fixture();

		public CalculatorStepDefinitions(ScenarioContext scenario) {
			this.scenario = scenario;
		}

		[Given("create a random object of type MyClass => {string}")]
		public void GivenCreateARandomObjectOfTypeMyClass(string myObject) {
			scenario.Set(fixture.Create<MyClass>(), myObject);
		}
		[Given("Verify that the Name property of the object {} equal {}")]
		public void GivenVerifyThatTheNamePropertyOfTheObjectMyObjectEqualContextMyObject_Name(MyClass myClass, object name) {
			name.Should().Be(myClass.Name);
		}

		[Given("Verify that the Value property of the object {} equal {int}")]
		public void GivenVerifyThatTheValuePropertyOfTheObjectMyObjectEqualContextMyObject_Value(MyClass myClass, int value) {
			value.Should().Be(myClass.Value);
		}

		[Given("Verify that the Age property of the object {} equal {}")]
		public void GivenVerifyThatTheAgePropertyOfTheObjectMyObjectEqualIntMyObject_Age(MyClass myClass, int? value) {
			value.Should().Be(myClass.Age);
		}
		[Then("Test the TimeOnly parameter: {}")]
		public void ThenTestTheTimeOnlyParameter(TimeOnly time) {
		}

		[StepArgumentTransformation(@"(.*)")]
		public MyClass MyClass(string key) => scenario.GetRequiredValue<MyClass>(key);
	}
}
