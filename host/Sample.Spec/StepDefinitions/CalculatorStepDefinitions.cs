using Microsoft.Extensions.Logging;

namespace Sample.Spec.StepDefinitions {
	[Binding]
	public sealed class CalculatorStepDefinitions {
		private readonly ILogger<CalculatorStepDefinitions> logger;
		private readonly FeatureContext feature;
		private readonly ScenarioContext scenario;

		public CalculatorStepDefinitions(ILogger<CalculatorStepDefinitions> logger, FeatureContext feature, ScenarioContext scenario) {
			this.logger = logger;
			this.feature = feature;
			this.scenario = scenario;
			logger.LogInformation("new instance CalculatorStepDefinitions: feature={feature} scenario={scenario}", 
				feature.FeatureInfo.Title, scenario.ScenarioInfo.Title);
		}

		[Given(@"a calculator")]
		public void GivenACalculator() {
			logger.LogInformation("given a calculator {context}", scenario.ScenarioInfo.Title);
		}


		[Given("the first number is (.*)")]
		public void GivenTheFirstNumberIs(int number) {
			logger.LogInformation("{title}: given the first number is {number}", scenario.ScenarioInfo.Title, number);
		}

		[Given("the second number is (.*)")]
		public void GivenTheSecondNumberIs(int number) {
			logger.LogInformation("given the second number is {number}", number);
		}

		[When("the two numbers are added")]
		public void WhenTheTwoNumbersAreAdded() {
			logger.LogInformation("when the two numbers are added");
		}

		[Then("the result should be (.*)")]
		public void ThenTheResultShouldBe(int result) {
			logger.LogInformation("then the result should be {result}", result);
		}

		[Given(@"my fav number is (.*)")]
		public void GivenMyFavNumberIs(int p0) {
		}

		[Then(@"is it odd or even\?")]
		public void ThenIsItOddOrEven() {
		}
	}
}
