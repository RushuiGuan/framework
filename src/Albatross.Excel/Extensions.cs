using Albatross.Excel.Table;
using ExcelDna.Integration;
using System;

namespace Albatross.Excel {
	public static class Extensions {
		/// <summary>
		/// convert an array into a two dimentional array that can be populated directly as an value in ExcelRange
		/// </summary>
		public static object[,] To2DimensionalArray(this object[] src, bool pivot = false) {
			object[,] result;
			if (pivot) {
				result = new object[src.Length, 1];
				for (int i = 0; i < src.Length; i++) {
					result[i, 0] = src[i];
				}
			} else {
				result = new object[1, src.Length];
				for (int i = 0; i < src.Length; i++) {
					result[0, i] = src[i];
				}
			}
			return result;
		}
		public static int? GetInteger(this object? input) {
			if (input is ExcelError || input == ExcelMissing.Value || input == ExcelEmpty.Value) {
				return null;
			} else if (input is double doubleValue) {
				return Convert.ToInt32(doubleValue);
			} else if (input is int intValue) {
				return intValue;
			} else {
				return null;
			}
		}
		public static double? GetDouble(this object? input) {
			if (input is ExcelError || input == ExcelMissing.Value || input == ExcelEmpty.Value) {
				return null;
			} else if (input is double) {
				return (double)input;
			} else {
				return null;
			}
		}
		public static DateTime? GetDateTime(this object? input) {
			if (input is ExcelError || input == ExcelMissing.Value || input == ExcelEmpty.Value) {
				return null;
			} else if (input is double value) {
				return DateTime.FromOADate(value);
			} else if (input is DateTime dateTime) {
				return dateTime;
			} else {
				var text = Convert.ToString(input);
				if (string.IsNullOrEmpty(text)) {
					return null;
				} else {
					try {
						return DateTime.Parse(text);
					} catch {
						return null;
					}
				}
			}
		}
		public static bool GetBoolean(this object input) {
			if (input is ExcelError || input == ExcelMissing.Value || input == ExcelEmpty.Value) {
				return false;
			} else if (input is bool value) {
				return value;
			} else {
				string text = Convert.ToString(input);
				if (string.IsNullOrEmpty(text)) {
					return false;
				} else {
					return bool.Parse(text);
				}
			}
		}
		public static bool IsExcelValue(this object input) {
			return input is ExcelError || input == ExcelMissing.Value || input == ExcelEmpty.Value;
		}
		public static CellBuilder FontProperties(this CellBuilder builder, Action<FontProperties> action)
			=> builder.Use(action);
		public static CellBuilder NumberFormat(this CellBuilder builder, Action<NumberFormat> action)
			=> builder.Use(action);
		public static CellBuilder Background(this CellBuilder builder, Action<Background> action)
			=> builder.Use(action);
		public static bool IsSingleCell(this ExcelReference range) => range.ColumnFirst == range.ColumnLast && range.RowFirst == range.RowLast;

		public static TableOptionsBuilder AddColumns(this TableOptionsBuilder builder, params TableColumn[] columns) {
			foreach (var item in columns) {
				builder.Add(item);
			}
			return builder;
		}
		public static void Clear(this ExcelReference range) {
			XlCall.Excel(XlCall.xlcSelect, range);
			XlCall.Excel(XlCall.xlcClear);
		}
	}
}
