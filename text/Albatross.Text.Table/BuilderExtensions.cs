using System;
using System.Reflection;

namespace Albatross.Text.Table {
	public static class BuilderExtensions {
		public static TableOptionBuilder<T> Format<T>(this TableOptionBuilder<T> buidler, string property, string format) {
			buidler.ColumnOptionBuilders[property].Formatter = (T entity, object? value) => string.Format($"{{0:{format}}}", value);
			return buidler;
		}
		public static TableOptionBuilder<T> Order<T>(this TableOptionBuilder<T> builder, string property, int order) {
			builder.ColumnOptionBuilders[property].GetOrder = () => order;
			return builder;
		}
		public static TableOptionBuilder<T> Header<T>(this TableOptionBuilder<T> builder, string property, string header) {
			builder.ColumnOptionBuilders[property].GetHeader = () => header;
			return builder;
		}
		public static string DefaultFormat(object? value) {
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

		public static TableOptionBuilder<T> SetByReflection<T>(this TableOptionBuilder<T> builder) {
			int index = 0;
			foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				int order = index++;
				builder.ColumnOptionBuilders[property.Name] = new TableColumnOptionBuilder<T> {
					GetValueDelegate = x => property.GetValue(x),
					Formatter = (T entity, object? value) => DefaultFormat(value),
					GetHeader = () => property.Name,
					GetOrder = () => order,
				};
			}
			return builder;
		}

		public static TableOptions<T> Build<T>(this TableOptionBuilder<T> builder) {
			return new TableOptions<T>(builder);
		}
	}
}