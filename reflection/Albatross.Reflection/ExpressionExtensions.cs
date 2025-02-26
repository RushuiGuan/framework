﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Albatross.Reflection {
	public static class ExpressionExtensions {
		public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T, object?>> lambda) {
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

		public static PropertyInfo GetPropertyInfo<T, V>(this Expression<Func<T, V>> lambda) {
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
		/// <summary>
		/// Provided with a type, a member name and a value, this method will return an predicate expression that checks the equality
		/// between instance member and the value.  The instance member can be a property or a field
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyOrFieldName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Expression<Func<T, bool>> GetPredicate<T>(string propertyOrFieldName, object? value) {
			ParameterExpression parameter = Expression.Parameter(typeof(T), "args");
			var body = Expression.Equal(Expression.PropertyOrField(parameter, propertyOrFieldName), Expression.Constant(value));
			return Expression.Lambda<Func<T, bool>>(body, parameter);
		}

		public static T SetValueIfNotNull<T, V>(this T ob, Expression<Func<T, V>> lambda, V? value) where V : struct {
			if (value.HasValue) {
				PropertyInfo prop = lambda.GetPropertyInfo();
				prop.SetValue(ob, value);
			}
			return ob;
		}

		public static T SetValueIfNotNull<T, V>(this T ob, Expression<Func<T, V?>> lambda, V? value) where V : struct {
			if (value.HasValue) {
				PropertyInfo prop = lambda.GetPropertyInfo();
				prop.SetValue(ob, value);
			}
			return ob;
		}
		public static T SetTextIfNotEmpty<T>(this T obj, Expression<Func<T, string?>> lambda, string? value) {
			if (!string.IsNullOrEmpty(value)) {
				PropertyInfo prop = lambda.GetPropertyInfo();
				prop.SetValue(obj, value);
			}
			return obj;
		}

		public static async Task SetValueIfHasKey<T, V, K>(this T ob, Expression<Func<T, V>> lambda, K? key, Func<K, Task<V>> func) where K : struct {
			if (key.HasValue) {
				var result = await func(key.Value);
				PropertyInfo prop = lambda.GetPropertyInfo();
				prop.SetValue(ob, result);
			}
		}
		public static async Task SetValueIfHasKey<T, V>(this T ob, Expression<Func<T, V>> lambda, string? value, Func<string, Task<V>> func) {
			if (!string.IsNullOrEmpty(value)) {
				var result = await func(value);
				PropertyInfo prop = lambda.GetPropertyInfo();
				prop.SetValue(ob, result);
			}
		}
	}
}