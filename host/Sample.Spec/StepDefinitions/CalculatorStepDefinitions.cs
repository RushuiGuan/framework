using Microsoft.Extensions.Logging;

namespace Sample.Spec.StepDefinitions {
	[Binding]
	public sealed class CalculatorStepDefinitions {
		private readonly ILogger<CalculatorStepDefinitions> logger;

		public CalculatorStepDefinitions(ILogger<CalculatorStepDefinitions> logger) {
			this.logger = logger;
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
	}
}
