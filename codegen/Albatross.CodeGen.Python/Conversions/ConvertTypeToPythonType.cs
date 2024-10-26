using Albatross.CodeGen.Python.Models;
using Albatross.Reflection;
using System;
using System.Collections;

namespace Albatross.CodeGen.Python.Conversions {
	public class ConvertTypeToPythonType : IConvertObject<Type, PythonType> {
		public PythonType Convert(Type type) {
			if (type.IsNullableValueType()) {
				return this.Convert(type.GetGenericArguments()[0]);
			} else if (type == typeof(string) || type == typeof(char)) {
				return My.Types.String();
			} else if (type == typeof(int) || type == typeof(long) || type == typeof(short) || type == typeof(byte)) {
				return My.Types.Int();
			} else if (type == typeof(float) || type == typeof(double)) {
				return My.Types.Float();
			} else if (type == typeof(decimal)) {
				return My.Types.Decimal();
			} else if (type == typeof(bool)) {
				return My.Types.Boolean();
			} else if (type == typeof(DateTime)) {
				return My.Types.DateTime();
			} else if (type == typeof(DateTimeOffset)) {
				return My.Types.DateTime();
			} else if (type == typeof(DateOnly)) {
				return My.Types.Date();
			} else if (type == typeof(TimeOnly)) {
				return My.Types.Time();
			} else if (type == typeof(TimeSpan)) {
				return My.Types.TimeDelta();
			} else if (type.IsArray || type is IEnumerable) {
				return My.Types.List();
			} else if (type.IsAssignableTo(typeof(IDictionary))) {
				return My.Types.Dictionary();
			} else {
				return My.Types.AnyType();
			}
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}