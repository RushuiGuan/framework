using System;
using TechTalk.SpecFlow;

namespace Sample.Spec.StepDefinitions
{
    [Binding]
    public class Feature1StepDefinitions
    {
        [Given(@"provide text = ""([^""]*)""")]
        public void GivenProvideText(string text)
        {
			Assert.Equal(text.Length, 2);
        }
    }
}
