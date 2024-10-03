using AutoFixture;
using System.Reflection;
using TechTalk.SpecFlow;

namespace Albatross.SpecFlow {
	public class ArgumentTransformations {
		public ArgumentTransformations(ScenarioContext scenario) {
			this.scenario = scenario;
		}

		protected readonly ScenarioContext scenario;
		public T GetRequiredValue<T>(string key) {
			if (scenario.TryGetValue<T>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}
		
		public T? GetPropertyValue<T>(string key, string propertyName) {
			if (scenario.TryGetValue(key, out var value)) {
				var type = value.GetType();
				var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty) ?? throw new ArgumentException($"Type {type.Name} doesn't have a public get property of name {propertyName}");
				if (property.PropertyType == typeof(T)) {
					return (T)property.GetValue(value);
				} else {
					throw new ArgumentException($"Property {propertyName} of type {type.Name} is not of type {typeof(T).Name}");
				}
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[Then(@"wait (.*) second\(s\)")]
		public Task ThenWaitSeconds(int seconds) => Task.Delay(seconds * 1000);

		#region random values
		[StepArgumentTransformation(@"random text")]
		public string AutoText() => new Fixture().Create<string>();

		[StepArgumentTransformation(@"random text \((.*)\)")]
		public string AutoText(int length) => new Fixture().Create<string>().Substring(0, length);

		[StepArgumentTransformation(@"random int between (.*) and (.*)")]
		public int AutoInt(int min, int max) => new Random().Next(min, max);
		#endregion

		#region dates
		[StepArgumentTransformation(@"today")]
		public DateOnly Today() => DateOnly.FromDateTime(DateTime.Today);

		[StepArgumentTransformation(@"(last saturday)")]
		public DateOnly LastSaturday(string key) {
			DateOnly lastSaturday = DateOnly.FromDateTime(DateTime.Today);
			while (lastSaturday.DayOfWeek != DayOfWeek.Saturday) {
				lastSaturday = lastSaturday.AddDays(-1);
			}
			return lastSaturday;
		}
		#endregion

		[StepArgumentTransformation(@"context:(.*)")]
		public string GetString(string key) => scenario.Get<string>(key);

		[StepArgumentTransformation(@"context:(.*)")]
		public int GetInt(string key) {
			if (scenario.TryGetValue<int>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}
	
		[StepArgumentTransformation(@"(with|without|should be|should not be|should|should not|active|inactive|yes|no)")]
		public bool BooleanTransform(string value) {
			return value == "with" || value == "should be" || value == "should" || value == "active" || value == "yes";
		}

		public T GetStoredValueByKey<T>(string value) {
			if (scenario.TryGetValue<T>(value, out var result)) {
				return result;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {value} and the type of {typeof(T).Name}");
			}
		}
	}
}
