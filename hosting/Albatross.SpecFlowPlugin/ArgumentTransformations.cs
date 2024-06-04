using AutoFixture;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Albatross.SpecFlowPlugin {
	public class ArgumentTransformations {
		protected readonly ScenarioContext scenario;

		public ArgumentTransformations(ScenarioContext scenario) {
			this.scenario = scenario;
		}

		[Then(@"wait (.*) second\(s\)")]
		public Task ThenWaitSeconds(int seconds) => Task.Delay(seconds * 1000);

		[StepArgumentTransformation(@"(with|without|should be|should not be)")]
		public bool BooleanTransform(string value) {
			return value == "with" || value == "should be";
		}

		[StepArgumentTransformation(@"random text")]
		public string AutoText() => new Fixture().Create<string>();

		[StepArgumentTransformation(@"random text with max length of (.*)")]
		public string AutoText(int length) => new Fixture().Create<string>().Substring(0, length);

		[StepArgumentTransformation(@"random int between (.*) and (.*)")]
		public int AutoInt(int min, int max) => new Random().Next(min, max);

		public T GetRequiredValue<T>(string key) {
			if (scenario.TryGetValue<T>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}
		public T GetRequiredPropertyValue<T>(string key) {
			if(scenario.TryGetValue(key, out var value)) {
				var type = value.GetType();
				var property = type.GetProperty(key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty) ?? throw new ArgumentException($"Type {type.Name} doesn't have a public get property of name {key}");
				if(property.PropertyType == typeof(T)) {
					return (T)property.GetValue(value);
				} else {
					throw new ArgumentException($"Property {key} of type {type.Name} is not of type {typeof(T).Name}");
				}
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}
	}
}
