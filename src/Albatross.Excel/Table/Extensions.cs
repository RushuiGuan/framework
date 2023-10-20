using Albatross.Reflection;
using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Windows;
using excel = Microsoft.Office.Interop.Excel;

namespace Albatross.Excel.Table {
	public static class Extensions {
		/// <summary>
		/// Read data from entity array of the generic type T and write to excel table using the <see cref="TableOptions"/> object
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values"></param>
		/// <param name="sheet"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static ExcelReference WriteTable<T>(this T[] values, excel.Worksheet sheet, TableOptions options) where T : notnull {
			if(options.TryGetDataRange(sheet.Name, out var range)) {
				range.Clear();
			}
			// create an 2D array that include the header
			object?[,] array = new object[values.Length + 1, options.Columns.Length];
			// set the header
			for (int columnIndex = 0; columnIndex < options.Columns.Length; columnIndex++) {
				array[0, columnIndex] = options.Columns[columnIndex].Title;
			}
			// populate the data
			for (int rowIndex = 1; rowIndex <= values.Length; rowIndex++) {
				T rowValue = values[rowIndex - 1];
				for (int columnIndex = 0; columnIndex < options.Columns.Length; columnIndex++) {
					var column = options.Columns[columnIndex];
					var cellValue = column.ReadEntityProperty(rowValue);
					array[rowIndex, columnIndex] = cellValue;
				}
			}
			// create a range for the whole table and set the value
			var tableRange = new ExcelReference(options.FirstRow, options.FirstRow + array.GetLength(0) - 1, options.FirstColumn, options.LastColumn, sheet.Name);
			tableRange.SetValue(array);
			// bold the header
			var headerRange = new ExcelReference(options.FirstRow, options.FirstRow, options.FirstColumn, options.LastColumn, sheet.Name);
			new CellBuilder(headerRange).FontProperties(x => x.Bold()).Apply();
			if (values.Length > 0) {
				// set the column styles
				foreach (var column in options.Columns) {
					var columnDataRange = new ExcelReference(tableRange.RowFirst + 1, tableRange.RowLast, tableRange.ColumnFirst + column.Index, tableRange.ColumnFirst + column.Index, sheet.Name);
					new CellBuilder(columnDataRange)
						.Use(column.NumberFormat)
						.Use(column.Background)
						.Use(column.FontProperties)
						.Use(column.Formula)
						.Apply();
				}
			}
			return tableRange;
		}

		public static void UpdateTable<T>(this T[] values, excel.Worksheet sheet, TableOptions options, bool setFormulas) where T : ExcelViewModel {
			object?[,] array;
			if (values.Length > 0) {
				values = values.OrderBy(args => args.RowIndex).ToArray();
				array = new object[1, options.Columns.Length];
				var changed = new List<ExcelReference>();
				for (int valueIndex = 0; valueIndex < values.Length; valueIndex++) {
					T rowValue = values[valueIndex];
					for (int columnIndex = 0; columnIndex < options.Columns.Length; columnIndex++) {
						var column = options.Columns[columnIndex];
						var cellValue = column.ReadEntityProperty(rowValue);
						array[0, columnIndex] = cellValue;
					}
					var rowRange = new ExcelReference(options.FirstRow + rowValue.RowIndex, options.FirstRow + rowValue.RowIndex,
						options.FirstColumn, options.LastColumn, sheet.Name);
					rowRange.SetValue(array);
					changed.Add(rowRange);
				}
				if (changed.Any()) {
					new CellBuilder(changed.Count == 1 ? changed.First() : new ExcelReference(changed)).FontProperties(x => x.Color(Color.Auto)).Apply();
				}
				if (setFormulas) {
					// reapply the formula
					var lastRowIndex = values.Last().RowIndex;
					foreach (var column in options.Columns) {
						var columnDataRange = new ExcelReference(options.FirstRow + 1, options.FirstRow + 1 + lastRowIndex,
							options.FirstColumn + column.Index, options.FirstColumn + column.Index, sheet.Name);
						new CellBuilder(columnDataRange)
							.Use(column.Formula)
							.Apply();
					}
				}
			}
		}
		/// <summary>
		/// Read data from excel and create a <see cref="ReadTableResult{T}"/>.  <see cref="ReadTableResult{T}"/> contains the resulting
		/// array of T as well as any read errors.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sheet"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static ReadTableResult<T> ReadTable<T>(this excel.Worksheet sheet, TableOptions options) where T : ExcelViewModel, new() {
			// get the last row of the table
			var lastRowIndex = options.GetLastRowIndex();
			var result = new ReadTableResult<T> {
				FirstRow = options.FirstRow,
				FirstColumn = options.FirstColumn,
				LastColumn = options.LastColumn,
				LastRow = lastRowIndex,
			};
			if (result.HasData) {
				// the the whole range into a 2D array
				object[,] data = (object[,])new ExcelReference(result.FirstRow, result.LastRow, result.FirstColumn, result.LastColumn, sheet.Name).GetValue();
				var errorCells = new List<ExcelReference>();
				for (int rowIndex = 1; rowIndex < data.GetLength(0); rowIndex++) {
					var t = new T {
						RowIndex = rowIndex,
					};
					var errors = new List<ReadError>();
					var absRowIndex = options.FirstRow + rowIndex;
					for (int columnIndex = 0; columnIndex < data.GetLength(1); columnIndex++) {
						var column = options.Columns[columnIndex];
						if (!column.ReadOnly) {
							var absColumnIndex = options.FirstColumn + columnIndex;
							if (!column.TrySetEntityProperty(t, data[rowIndex, columnIndex], out var error)) {
								errors.Add(new ReadError(absRowIndex, absColumnIndex, error));
								errorCells.Add(new ExcelReference(absRowIndex, absRowIndex, absColumnIndex, absColumnIndex, sheet.Name));
							}
						}
					}
					if (errors.Any()) {
						result.Errors.AddRange(errors);
					} else {
						result.Data.Add(t);
					}
				}
				if (errorCells.Any()) {
					new CellBuilder(new ExcelReference(errorCells)).FontProperties(x => x.Color(Color.Blue)).Apply();
				}
			}
			return result;
		}
		/// <summary>
		/// return the last row index of a table using a key column.
		/// This method will move current selection
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public static int GetLastRowIndex(this TableOptions options) {
			var keyColumn = options.Columns.Where(args => args.IsKey).FirstOrDefault() ?? throw new InvalidOperationException("table option has no key column");
			var cell = new ExcelReference(options.FirstRow, options.FirstRow, options.FirstColumn + keyColumn.Index, options.FirstColumn + keyColumn.Index);
			if (cell.TryFindLastRow(out var lastRowIndex)) {
				return lastRowIndex.Value;
			} else {
				return options.FirstRow;
			}
		}
		public static bool TryGetDataRange(this TableOptions options, string sheet, [NotNullWhen(true)] out ExcelReference? range) {
			var lastRowIndex = GetLastRowIndex(options);
			if (lastRowIndex == options.FirstRow) {
				range = null;
				return false;
			} else {
				range = new ExcelReference(options.FirstRow + 1, lastRowIndex, options.FirstColumn, options.LastColumn, sheet);
				return true;
			}
		}
		/// <summary>
		/// Return the range of a table column excluding its header
		/// </summary>
		/// <param name="tableRange"></param>
		/// <param name="options"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static ExcelReference GetColumnRange(this ExcelReference tableRange, TableOptions options, string name) {
			var index = -1;
			for (int i = 0; i < options.Columns.Length; i++) {
				if (options.Columns[i].Name == name) {
					index = i;
					break;
				}
			}
			if (index == -1) { throw new ArgumentException($"{name} is not a valid table column name"); }
			return new ExcelReference(tableRange.RowFirst + 1, tableRange.RowLast, tableRange.ColumnFirst + index, tableRange.ColumnFirst + index);
		}
		public static TableColumn Use(this TableColumn column, TrySetEntityPropertyDelegate trySetValueDelegate) {
			column.ReadOnly = false;
			column.TrySetEntityPropertyHandler = trySetValueDelegate;
			return column;
		}
		public static TableColumn UseSetEntityPropertyByReflection(this TableColumn column) => column.Use(TrySetEntityPropertyByReflection);
		public static bool TrySetEntityPropertyByReflection(object entity, TableColumn column, object cellValue, [NotNullWhen(false)] out string? error) {
			error = null;
			var type = entity.GetType();
			PropertyInfo propertyInfo = type.GetProperty(column.Name);
			if (propertyInfo != null) {
				if (cellValue == null || cellValue == ExcelMissing.Value || cellValue == ExcelEmpty.Value || cellValue is ExcelError && column.UseNullForError) {
					if (column.IsNullable) {
						propertyInfo.SetValue(entity, null);
						return true;
					} else {
						error = $"Property {column.Name} cannot be null";
						return false;
					}
				} else if (cellValue is ExcelError excelError) {
					error = $"{excelError}";
					return false;
				} else if (cellValue.GetType() == column.Type) {
					propertyInfo.SetValue(entity, cellValue);
					return true;
				} else {
					try {
						if (column.Type == typeof(DateTime)) {
							cellValue = cellValue.GetDateTime();
						} else {
							cellValue = Convert.ChangeType(cellValue, column.Type);
						}
						if (cellValue == null && !column.IsNullable) {
							error = $"Property {column.Name} cannot be null";
							return false;
						} else {
							propertyInfo.SetValue(entity, cellValue);
							return true;
						}
					} catch {
						error = $"cannot change {cellValue} to {column.Type.Name}";
						return false;
					}
				}
			} else {
				error = $"{column.Name} is not a property of type {type.Name}";
				return false;
			}
		}

		public static TableColumn UseDefaultReadEntityPropertyHandler(this TableColumn tableColumn) {
			tableColumn.ReadEntityPropertyHandler = (column, entity) => entity.GetType().GetPropertyValue(entity, column.Name);
			return tableColumn;
		}
		public static object? ReadEntityPropertyDefaultHandler(TableColumn column, object entity) {
			return entity.GetType().GetPropertyValue(entity, column.Name);
		}

		public static TableColumn Required(this TableColumn tableColumn, bool required = true) {
			tableColumn.Required = required;
			return tableColumn;
		}
		public static bool TryUseCurrentSelection(this TableOptions options, int rowCount) {
			var sheet = My.ActiveWorksheet();
			var selection = My.ActiveSelection();
			if (sheet != null && selection != null) {
				options.FirstColumn = selection.ColumnFirst;
				options.FirstRow = selection.RowFirst;
				var targetRange = new ExcelReference(options.FirstRow, options.FirstRow + rowCount, options.FirstColumn, options.FirstColumn + options.Columns.Length - 1, sheet.Name);
				var values = (object[,])targetRange.GetValue();
				if (values.HasData()) {
					var result = MessageBox.Show("Target range contains data that will be overwritten and the process cannot be undone.  Do you want to proceed?", "Warning", MessageBoxButton.YesNo);
					if (result == MessageBoxResult.Yes) {
						return true;
					}
				} else {
					return true;
				}
			}
			return false;
		}
	}
}