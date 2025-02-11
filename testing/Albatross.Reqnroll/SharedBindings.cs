using Albatross.Dates;
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
		[StepArgumentTransformation(@"now")]
		public DateTime Now() => DateTime.Now;

		[StepArgumentTransformation(@"utcnow")]
		public DateTime UtcNow() => DateTime.UtcNow;

		[StepArgumentTransformation(@"today")]
		public DateOnly Today() => DateOnly.FromDateTime(DateTime.Today);

		[StepArgumentTransformation(@"last saturday")]
		public DateOnly LastSaturday() {
			DateOnly lastSaturday = DateOnly.FromDateTime(DateTime.Today);
			while (lastSaturday.DayOfWeek != DayOfWeek.Saturday) {
				lastSaturday = lastSaturday.AddDays(-1);
			}
			return lastSaturday;
		}

		[StepArgumentTransformation(@"previousWeekday")]
		public DateOnly PreviousWeekday() {
			return DateTime.Today.DateOnly().PreviousWeekday();
		}
		#endregion

		#region context based transformations
		[StepArgumentTransformation(@"(with|without|should be|should not be|should|should not|active|inactive|yes|no|including|excluding)")]
		public bool BooleanTransform(string value) {
			return value == "with" || value == "should be" || value == "should" || value == "active" || value == "yes" || value == "including";
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public object GetValue(string key) => scenario[key];

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

		[StepArgumentTransformation(@"context:(\w+)")]
		public bool GetBool(string key) {
			if (scenario.TryGetValue<bool>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public long GetLong(string key) {
			if (scenario.TryGetValue<long>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public decimal GetDecimal(string key) {
			if (scenario.TryGetValue<decimal>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public double GetDouble(string key) {
			if (scenario.TryGetValue<double>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public DateTime GetDateTime(string key) {
			if (scenario.TryGetValue<DateTime>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public DateOnly GetDateOnly(string key) {
			if (scenario.TryGetValue<DateOnly>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public int? GetNullableInt(string key) {
			if (scenario.TryGetValue<int?>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public bool? GetNullableBool(string key) {
			if (scenario.TryGetValue<bool?>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public long? GetNullableLong(string key) {
			if (scenario.TryGetValue<long?>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public decimal? GetNullableDecimal(string key) {
			if (scenario.TryGetValue<decimal?>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public double? GetNullableDouble(string key) {
			if (scenario.TryGetValue<double?>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public DateTime? GetNullableDateTime(string key) {
			if (scenario.TryGetValue<DateTime?>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)")]
		public DateOnly? GetNullableDateOnly(string key) {
			if (scenario.TryGetValue<DateOnly?>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public object? GetProperty(string key, string property) {
			if (scenario.TryGetValue(key, out var value)) {
				var type = value.GetType();
				var propertyInfo = type.GetProperty(property);
				if (propertyInfo == null) {
					throw new ArgumentException($"Type {type.Name} doesn't have a public get property of name {property}");
				} else {
					return propertyInfo.GetValue(value);
				}
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public string? GetStringProperty(string key, string property) => scenario.GetPropertyValue<string>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public bool GetBooleanProperty(string key, string property) => scenario.GetPropertyValue<bool>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public int GetIntProperty(string key, string property) => scenario.GetPropertyValue<int>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public long GetLongProperty(string key, string property) => scenario.GetPropertyValue<long>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public decimal GetDecimalProperty(string key, string property) => scenario.GetPropertyValue<decimal>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public double GetDoubleProperty(string key, string property) => scenario.GetPropertyValue<double>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public DateTime GetDateTimeProperty(string key, string property) => scenario.GetPropertyValue<DateTime>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public DateOnly GetDateOnlyProperty(string key, string property) => scenario.GetPropertyValue<DateOnly>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public int? GetNullableIntProperty(string key, string property) => scenario.GetPropertyValue<int?>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public bool? GetNullableBoolProperty(string key, string property) => scenario.GetPropertyValue<bool?>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public long? GetNullableLongProperty(string key, string property) => scenario.GetPropertyValue<long?>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public decimal? GetNullableDecimalProperty(string key, string property) => scenario.GetPropertyValue<decimal?>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public double? GetNullableDoubleProperty(string key, string property) => scenario.GetPropertyValue<double?>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public DateTime? GetNullableDateTimeProperty(string key, string property) => scenario.GetPropertyValue<DateTime?>(key, property);

		[StepArgumentTransformation(@"context:(\w+)\.(\w+)")]
		public DateOnly? GetNullableDateProperty(string key, string property) => scenario.GetPropertyValue<DateOnly?>(key, property);
		#endregion

		[Then(@"Show value of {Object}")]
		public void ShowValue(object value) {
			Console.WriteLine(value);
		}
	}
}