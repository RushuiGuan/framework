using Albatross.Text;
using System.Reflection;

namespace Albatross.Text.Table {
	public class TableOptionBuilder<T> {
		public Dictionary<string, Func<T, object?>> GetValueDelegates { get; } = new Dictionary<string, Func<T, object?>>();
		public Dictionary<string, Func<T, object?, string>> Formatters { get; } = new Dictionary<string, Func<T, object?, string>>();
		public Dictionary<string, string> ColumnHeaders { get; } = new Dictionary<string, string>();
		public Dictionary<string, int> ColumnOrders { get; } = new Dictionary<string, int>();

		public TableOptionBuilder() {
			int index = 0;
			foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				GetValueDelegates[property.Name] = x => property.GetValue(x);
				Formatters[property.Name] = (T entity, object? value) => DefaultFormat(value);
				ColumnHeaders[property.Name] = property.Name;
				ColumnOrders[property.Name] = index++;
			}
		}

		public TableOptionBuilder<T> ColumnOrder(string property, int index) {
			this.ColumnOrders[property] = index;
			return this;
		}

		public TableOptionBuilder<T> Format(string property, string format) {
			this.Formatters[property] = (T entity, object? value) => string.Format($"{{0:{format}}}", value);
			return this;
		}

		public TableOptionBuilder<T> Exclude(string property) {
			this.GetValueDelegates.Remove(property);
			this.Formatters.Remove(property);
			this.ColumnHeaders.Remove(property);
			this.ColumnOrders.Remove(property);
			return this;
		}

		public TableOptionBuilder<T> SetColumn(string column, Func<T, object?> getValue) {
			this.GetValueDelegates[column] = getValue;
			if (!this.Formatters.ContainsKey(column)) {
				this.Formatters[column] = (T entity, object? value) => DefaultFormat(value);
			}
			if (!this.ColumnHeaders.ContainsKey(column)) {
				this.ColumnHeaders[column] = column;
			}
			if (!this.ColumnOrders.ContainsKey(column)) {
				this.ColumnOrders[column] = this.ColumnOrders.Count;
			}
			return this;
		}

		public TableOptionBuilder<T> ColumnHeader(string property, string columnHeader) {
			this.ColumnHeaders[property] = columnHeader;
			return this;
		}

		public TableOptionBuilder<T> Format(string property, Func<T, object?, string> format) {
			this.Formatters[property] = format;
			return this;
		}

		static string DefaultFormat(object? value) {
			if (value == null) {
				return string.Empty;
			} else {
				// note that an object can never contain an instance of Nullable struct
				switch (value) {
					case DateOnly date:
						return $"{date:yyyy-MM-dd}";
					case TimeOnly time:
						return $"{time:HH:mm:ss}";
					case DateTime dateTime:
						return $"{dateTime:yyyy-MM-ddTHH:mm:ssK}";
					case DateTimeOffset dateTimeOffset:
						return $"{dateTimeOffset:yyyy-MM-ddTHH:mm:ssK}";
					case decimal d:
						return d.Decimal2CompactText();
					default:
						return Convert.ToString(value) ?? string.Empty;
				}
			}
		}
	}
}
