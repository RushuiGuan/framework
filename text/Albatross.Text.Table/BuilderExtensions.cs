using Albatross.Reflection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Albatross.Text.Table {
	public static class BuilderExtensions {
		public static TableOptionBuilder<T> Format<T, P>(this TableOptionBuilder<T> builder, Expression<Func<T, P>> lambda, string format)
			=> builder.Format(lambda, (T entity, object? value) => string.Format($"{{0:{format}}}", value));

		public static TableOptionBuilder<T> Format<T, P>(this TableOptionBuilder<T> buidler, Expression<Func<T, P>> lambda, Func<T, object?, string> format)
			=> buidler.Format(lambda.GetPropertyInfo().Name, format);

		public static TableOptionBuilder<T> ColumnOrder<T, P>(this TableOptionBuilder<T> builder, Expression<Func<T, P>> lambda, Func<int> getOrder)
			=> builder.ColumnOrder(lambda.GetPropertyInfo().Name, getOrder);

		public static TableOptionBuilder<T> ColumnOrder<T, P>(this TableOptionBuilder<T> builder, Expression<Func<T, P>> lambda, int order)
			=> builder.ColumnOrder(lambda.GetPropertyInfo().Name, () => order);

		public static TableOptionBuilder<T> ColumnHeader<T, P>(this TableOptionBuilder<T> builder, Expression<Func<T, P>> lambda, Func<string> getColumnHeader)
			=> builder.ColumnHeader(lambda.GetPropertyInfo().Name, getColumnHeader);

		public static TableOptionBuilder<T> ColumnHeader<T, P>(this TableOptionBuilder<T> builder, Expression<Func<T, P>> lambda, string header)
			=> builder.ColumnHeader(lambda.GetPropertyInfo().Name, () => header);

		public static TableOptionBuilder<T> Ignore<T, P>(this TableOptionBuilder<T> builder, Expression<Func<T, P>> lambda)
			=> builder.Ignore(lambda.GetPropertyInfo().Name);

		public static TableOptionBuilder<T> Property<T, P>(this TableOptionBuilder<T> builder, Expression<Func<T, P>> lambda, Func<T, object?>? getValue = null) {
			var propertyInfo = lambda.GetPropertyInfo();
			return builder.SetColumn(propertyInfo.Name, getValue ?? (x => propertyInfo.GetValue(x)));
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

		public static TableOptionBuilder<T> AddPropertiesByReflection<T>(this TableOptionBuilder<T> builder) {
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