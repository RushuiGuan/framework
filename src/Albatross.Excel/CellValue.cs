using ExcelDna.Integration;
using System;

namespace Albatross.Excel {
	public static class CellValue {
		public static bool TryReadInteger(object input, out int value) {
			value = 0;
			if (IsError(input) || IsEmpty(input)) {
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
			if (IsError(input) || IsEmpty(input)) {
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
			if (IsError(input) || IsEmpty(input)) {
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
			if (IsError(input) || IsEmpty(input)) {
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
			if (IsError(input) || IsEmpty(input)) {
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
			if (IsError(input) || IsEmpty(input)) {
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
			if (IsError(input)) {
				return false;
			}else if(IsEmpty(input)) {
				return true;
			} else { 
				value = Convert.ToString(value) ?? string.Empty;
				return true;
			}
		}
		public static bool IsError(object cellValue) => cellValue is ExcelError;
		public static bool IsEmpty(object cellValue) => cellValue == ExcelMissing.Value || cellValue == ExcelEmpty.Value;
		public static bool IsEmptyArray(object[,] values) {
			for (int rowIndex = 0; rowIndex < values.GetLength(0); rowIndex++) {
				for (int columnIndex = 0; columnIndex < values.GetLength(1); columnIndex++) {
					if(!IsEmpty(values[rowIndex, columnIndex])) {
						return false;
					}
				}
			}
			return true;
		}
	}
}