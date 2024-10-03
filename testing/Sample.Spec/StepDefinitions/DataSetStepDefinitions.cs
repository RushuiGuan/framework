using Microsoft.Extensions.Logging;
using System;
using TechTalk.SpecFlow;

namespace Sample.Spec.StepDefinitions
{
    [Binding]
    public class DataSetStepDefinitions
    {
		private readonly ILogger<DataSetStepDefinitions> logger;
		private readonly FeatureContext feature;
		private readonly ScenarioContext scenario;

		public DataSetStepDefinitions(ILogger<DataSetStepDefinitions> logger, FeatureContext feature, ScenarioContext scenario) {
			this.logger = logger;
			this.feature = feature;
			this.scenario = scenario;
			logger.LogInformation("new instance DataSetStepDefinitions: feature={feature} scenario={scenario}", 
				feature.FeatureInfo.Title, scenario.ScenarioInfo.Title);
		}

		[Given(@"a id of (.*)")]
        public void GivenAIdOf(int p0)
        {
			logger.LogInformation("{context}: given a id", scenario.ScenarioInfo.Title);
        }

        [When(@"save the dataset")]
        public void WhenSaveTheDataset()
        {
			logger.LogInformation("{context}: WhenSaveTheDataset", scenario.ScenarioInfo.Title);
		}

        [Then(@"a new dataset is created with the id (.*)")]
        public void ThenANewDatasetIsCreatedWithTheId(int p0)
        {
			logger.LogInformation("{context}: ThenANewDatasetIsCreatedWithTheId", scenario.ScenarioInfo.Title);
		}
    }
}
