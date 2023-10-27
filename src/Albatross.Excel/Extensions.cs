using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Albatross.Excel {
	public static class Extensions {
		public static CellBuilder FontProperties(this CellBuilder builder, Action<FontProperties> action)
			=> builder.Use(action);
		public static CellBuilder NumberFormat(this CellBuilder builder, Action<NumberFormat> action)
			=> builder.Use(action);
		public static CellBuilder Background(this CellBuilder builder, Action<Background> action)
			=> builder.Use(action);

		public static bool IsSingleCell(this ExcelReference range) => range.ColumnFirst == range.ColumnLast && range.RowFirst == range.RowLast;
		public static void Clear(this ExcelReference range) {
			XlCall.Excel(XlCall.xlcSelect, range);
			XlCall.Excel(XlCall.xlcClear);
		}
		public static object[,] CreateRangeValue<T>(this IEnumerable<T> items, string[] headers, params Func<T, object?>[] getValues) {
			var data = items.ToArray();
			if(headers.Length < getValues.Length) {
				throw new ArgumentException("Header length is less than getValues length");
			}
			object[,] result = new object[data.Length + 1, getValues.Length];
			for (int columnIndex = 0; columnIndex < getValues.Length; columnIndex++) {
				result[0, columnIndex] = headers[columnIndex];
			}
			for (int rowIndex = 0; rowIndex < data.Length; rowIndex++) {
				for (int columnIndex = 0; columnIndex < getValues.Length; columnIndex++) {
					result[rowIndex + 1, columnIndex] = CellValue.Write(getValues[columnIndex](data[rowIndex]));
				}
			}
			return result;
		}

		public static object[,] CreateRangeValueByReflection<T>(this IEnumerable<T> items, params string[] fields) {
			var data = items.ToArray();
			var type = typeof(T);
			var properties = new List<PropertyInfo>();
			if (fields.Length != 0) {
				foreach (var field in fields) {
					properties.Add(type.GetProperty(field, BindingFlags.Public | BindingFlags.Instance)
						?? throw new ArgumentException($"{field} is not a valid public, instance property for {type.Name}"));
				}
			} else {
				properties.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance));
			}
			var result = new object[data.Length + 1, properties.Count];
			for (int columnIndex = 0; columnIndex < properties.Count; columnIndex++) {
				result[0, columnIndex] = properties[columnIndex].Name;
			}
			for (int rowIndex = 0; rowIndex < data.Length; rowIndex++) {
				for (int columnIndex = 0; columnIndex < properties.Count; columnIndex++) {
					result[rowIndex + 1, columnIndex] = CellValue.Write(properties[columnIndex].GetValue(data[rowIndex]));
				}
			}
			return result;
		}
	}
}
