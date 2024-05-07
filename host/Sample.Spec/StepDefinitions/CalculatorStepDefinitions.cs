using Microsoft.Extensions.Logging;

namespace Sample.Spec.StepDefinitions {
	[Binding]
	public sealed class CalculatorStepDefinitions {
		private readonly ILogger<CalculatorStepDefinitions> logger;

		public CalculatorStepDefinitions(ILogger<CalculatorStepDefinitions> logger) {
			this.logger = logger;
			logger.LogInformation("creating CalculatorStepDefinitions");
		}

		[Given(@"a calculator")]
		public void GivenACalculator() {
			logger.LogInformation("given a calculator");
		}


		[Given("the first number is (.*)")]
		public void GivenTheFirstNumberIs(int number) {
			logger.LogInformation("given the first number is {number}", number);
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
