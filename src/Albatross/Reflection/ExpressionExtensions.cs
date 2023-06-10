using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Albatross.Reflection {
	public static class ExpressionExtensions {
		public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T, object>> lambda) {
			MemberExpression? member = lambda.Body as MemberExpression;
			if (member == null) {
				throw new ArgumentException($"Expression '{lambda}' refers to a method.");
			}

			PropertyInfo? propInfo = member.Member as PropertyInfo;
			if (propInfo == null) {
				throw new ArgumentException($"Expression '{lambda}' refers to a field, not a property.");
			}
			return propInfo;
		}

		public static Expression<Func<T, bool>> GetPredicate<T>(string propertyOrFieldName, object? value) {
			ParameterExpression parameter = Expression.Parameter(typeof(T), "args");
			var body = Expression.Equal(Expression.PropertyOrField(parameter, propertyOrFieldName), Expression.Constant(value));
			return Expression.Lambda<Func<T, bool>>(body, parameter);
		}
	}
}
