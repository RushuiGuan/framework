using ExcelDna.Integration;
using System;
using System.Text.Json;

namespace Albatross.Excel {
	public static class CellValue {
		public static bool TryReadInteger(object input, out int value) {
			value = 0;
			if (IsBuiltIn(input)) {
				return false;
			} else if (input is double doubleValue) {
				value = Convert.ToInt32(doubleValue);
				return true;
			} else if (input is int intValue) {
				value = intValue;
				return true;
			} else {
				return false;
			}
		}
		public static bool TryReadDouble(object input, out double value) {
			value = 0;
			if (IsBuiltIn(input)) {
				return false;
			} else if (input is double) {
				value = (double)input;
				return true;
			} else {
				return false;
			}
		}
		public static bool TryReadDateTime(object input, out DateTime dateTime) {
			dateTime = DateTime.MinValue;
			if (IsBuiltIn(input)) {
				return false;
			} else if (input is double value) {
				dateTime = DateTime.FromOADate(value);
				return true;
			} else if (input is DateTime dateTimeValue) {
				dateTime = dateTimeValue;
				return true;
			} else {
				var text = Convert.ToString(input);
				if (string.IsNullOrEmpty(text)) {
					return false;
				} else {
					try {
						dateTime = DateTime.Parse(text);
						return false;
					} catch {
						return false;
					}
				}
			}
		}
		public static bool TryReadDateOnly(object input, out DateOnly date) {
			date = DateOnly.MinValue;
			if (IsBuiltIn(input)) {
				return false;
			} else if (input is double value) {
				date = DateOnly.FromDateTime(DateTime.FromOADate(value));
				return true;
			} else if (input is DateTime dateTimeValue) {
				date = DateOnly.FromDateTime(dateTimeValue);
				return true;
			} else {
				var text = Convert.ToString(input);
				if (string.IsNullOrEmpty(text)) {
					return false;
				} else {
					try {
						date = DateOnly.Parse(text);
						return false;
					} catch {
						return false;
					}
				}
			}
		}
		public static bool TryReadTimeOnly(object input, out TimeOnly date) {
			date = TimeOnly.MinValue;
			if (IsBuiltIn(input)) {
				return false;
			} else if (input is double value) {
				date = TimeOnly.FromDateTime(DateTime.FromOADate(value));
				return true;
			} else if (input is DateTime dateTimeValue) {
				date = TimeOnly.FromDateTime(dateTimeValue);
				return true;
			} else {
				var text = Convert.ToString(input);
				if (string.IsNullOrEmpty(text)) {
					return false;
				} else {
					try {
						date = TimeOnly.Parse(text);
						return false;
					} catch {
						return false;
					}
				}
			}
		}
		public static bool TryReadBoolean(object input, out bool value) {
			value = false;
			if (IsBuiltIn(input)) {
					return false;
			} else if (input is bool cellValue) {
				value = cellValue;
				return true;
			} else {
				string text = Convert.ToString(input);
				switch (text?.ToLower() ?? string.Empty) {
					case "1":
					case "true":
					case "yes":
						value = true;
						return true;
					case "0":
					case "false":
					case "no":
						value = false;
						return true;
					default:
						return false;
				}
			}
		}
		public static bool TryReadString(object input, out string value) {
			value = string.Empty;
			if(input == ExcelEmpty.Value || input == ExcelMissing.Value) {
				return true;
			}else if(input is ExcelError) {
				return false;
			} else {
				value = Convert.ToString(value) ?? string.Empty;
				return true;
			}
		}
		public static bool IsBuiltIn(object cellValue) => cellValue is ExcelError || cellValue == ExcelMissing.Value || cellValue == ExcelEmpty.Value;
		public static bool HasData(object[,] values) {
			for (int i = 0; i < values.GetLength(0); i++) {
				for (int j = 0; j < values.GetLength(1); j++) {
					if (values[i, j] != ExcelEmpty.Value) {
						return true;
					}
				}
			}
			return false;
		}

		//public static bool TryRead(object input, Type type, bool json, out object value) { 
		//	if(input is ExcelError excelError) { }
		//	if(type ==  typeof(string)) {
		//		return TryReadString(input, out value);
		//	}else if(json) {
		//		JsonSerializer.Deserialize()
		//	} else {
		//	}
		//}
	}
}