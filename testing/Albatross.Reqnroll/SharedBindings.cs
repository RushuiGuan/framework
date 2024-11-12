using AutoFixture;
using Reqnroll;

namespace Albatross.Reqnroll {
	public class SharedBindings {
		protected readonly ScenarioContext scenario;
		static Random random = new Random();
		public SharedBindings(ScenarioContext scenario) {
			this.scenario = scenario;
		}

		[Then(@"Wait (\d+) second\(s\)")]
		public Task ThenWaitSeconds(int seconds) => Task.Delay(seconds * 1000);

		#region random values
		[StepArgumentTransformation(@"random text")]
		public string RandomText() => new Fixture().Create<string>();

		[StepArgumentTransformation(@"random int")]
		public int RandomInt() => random.Next();

		[StepArgumentTransformation(@"random text\s*\((\d+)\)")]
		public string RandomText(int length) => new Fixture().Create<string>().Substring(0, length);

		[StepArgumentTransformation(@"random int\s*\(\s*(\d+)\s*-\s*(\d+)\s*\)")]
		public int RandomInt(int min, int max) => random.Next(min, max);
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

		[StepArgumentTransformation(@"context:(\w+)")]
		public string GetString(string key) => scenario.Get<string>(key);

		[StepArgumentTransformation(@"context:(\w+)")]
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

		[StepArgumentTransformation(@"context: (\w+)\.(\w+)")]
		public object? GetPropertyValue(string key, string property) {
			if (scenario.TryGetValue(key, out var value)) {
				var type = value.GetType();
				var propertyInfo = type.GetProperty(property);
				if (propertyInfo == null) {
					throw new ArgumentException($"Type {type.Name} doesn't have a public get property of name {property}");
				}else{
					return propertyInfo.GetValue(value);
				}
			} else{
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[Then("show value => (.*)")]
		public void ShowValue(object value) { }
	}
}