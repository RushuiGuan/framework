using Albatross.Text.Table;
using Reqnroll;
using System.Reflection;

namespace Albatross.Reqnroll {
	public static class Extensions {
		public static T GetRequiredValue<T>(this ScenarioContext scenario, string key) {
			if (scenario.TryGetValue<T>(key, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"ScenarioContext doesn't have a value with the name of {key}");
			}
		}

		public static T? GetPropertyValue<T>(this ScenarioContext scenario, string key, string propertyName) {
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

		public static DataTable DataTable<T>(this IEnumerable<T> items, TableOptions<T>? options = null) {
			options = options ?? TableOptionFactory.Instance.Get<T>();
			var table = new DataTable(options.ColumnOptions.Select(x=>x.Header).ToArray());
			foreach (var item in items) {
				table.AddRow(options.GetValue(item));
			}
			return table;
		}
	}
}
