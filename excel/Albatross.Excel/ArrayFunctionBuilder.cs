using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Albatross.Excel {
	public record class ArrayFunctionColumn {
		public int Index { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public Func<object?, object?> Func { get; set; }
		public ArrayFunctionColumn(string name, string? title, Func<object?, object?> func) {
			this.Name = name;
			this.Title = title ?? name;
			this.Func = func;
		}
	}
	public class ArrayFunctionBuilder {
		Type type;
		bool hasHeader;
		public ExcelReference Caller { get; }
		string[] orderedColumnNames = Array.Empty<string>();
		ExcelAction? actions;
		Dictionary<string, ArrayFunctionColumn> columns = new Dictionary<string, ArrayFunctionColumn>();
		public object[,] Result { get; private set; } = new object[0, 0];
		public int ItemCount { get; private set; }

		public ArrayFunctionBuilder(Type type, bool hasHeader) {
			this.type = type;
			this.hasHeader = hasHeader;
			this.Caller = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
		}
		public ArrayFunctionBuilder Queue(ExcelAction action) {
			if (actions == null) {
				actions = action;
			} else {
				actions += action;
			}
			return this;
		}
		public ArrayFunctionBuilder HasHeader(bool has) {
			this.hasHeader = has;
			return this;
		}
		public ArrayFunctionBuilder RestoreSelection() {
			this.Queue(() => this.Caller.Select());
			return this;
		}

		public object[,] SetValue<T>(IEnumerable<T> values) {
			this.Build();
			if (type.IsAssignableTo(typeof(T))) {
				var array = values.ToArray();
				int offset = 0;
				if (hasHeader) { offset = 1; }
				Result = new object[array.Length + offset, this.columns.Count];
				if (hasHeader) {
					foreach (var column in columns.Values) {
						Result[0, column.Index] = column.Title;
					}
				}
				for (int rowIndex = 0; rowIndex < array.Length; rowIndex++) {
					var rowValue = array[rowIndex];
					foreach (var column in columns.Values) {
						var value = column.Func(rowValue);
						Result[rowIndex + offset, column.Index] = CellValue.Write(value);
					}
				}
				this.ItemCount = array.Length;
				ApplyActions();
				return Result;
			} else {
				throw new ArgumentException($"ArrayFunction value has to be assignable from type {type.FullName}");
			}
		}
		private void ApplyActions() {
			if (this.actions != null) {
				ExcelAsyncUtil.QueueAsMacro(this.actions);
			}
		}
		public ArrayFunctionBuilder BoldHeader() => this.FormatHeader(cellBuilder => cellBuilder.FontProperties(x => x.Bold()));
		public ArrayFunctionBuilder DefaultFormatHeader() => this.BoldHeader().RestoreSelection();

		public ArrayFunctionBuilder AddColumn(ArrayFunctionColumn column) {
			columns.Add(column.Name, column);
			column.Index = columns.Count;
			return this;
		}
		public ArrayFunctionBuilder AddColumn(string name, string? title, Func<object?, object?> func) => AddColumn(new ArrayFunctionColumn(name, title, func));
		public ArrayFunctionBuilder AddColumnsByReflection(bool includeInheritedProperties = false, params string[] fields) {
			if (fields.Length != 0) {
				foreach (var field in fields) {
					var option = BindingFlags.Public | BindingFlags.Instance;
					if (includeInheritedProperties) { option = option | BindingFlags.FlattenHierarchy; }
					var info = this.type.GetProperty(field, option)
						?? throw new ArgumentException($"{field} is not a valid public, instance property for {type.Name}");
					AddColumn(info.Name, null, entity => info.GetValue(entity));
				}
			} else {
				foreach (var info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
					AddColumn(info.Name, null, entity => info.GetValue(entity));
				}
			}
			return this;
		}
		/// <summary>
		/// set the order the columns.  the specified columns will be created ahead of the remaining columns.  the remaining columns will be displayed
		/// based on the original insertion order.
		/// </summary>
		/// <param name="names"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public ArrayFunctionBuilder SetOrder(params string[] names) {
			if (names.Distinct().Count() != names.Length) { throw new ArgumentException("Duplicate column names found"); }
			this.orderedColumnNames = names;
			return this;
		}
		public ArrayFunctionBuilder FormatColumns(Action<CellBuilder> action, params string[] columnNames) {
			if (columnNames.Length == 0) {
				throw new ArgumentException("Missing columns names");
			}
			this.Queue(() => {
				if (this.ItemCount > 0) {
					var ranges = new List<ExcelReference>();
					var offset = this.hasHeader ? 1 : 0;
					foreach (var name in columnNames) {
						if (!columns.TryGetValue(name, out var column)) {
							throw new InvalidOperationException($"{name} is not an existing column");
						}
						ranges.Add(new ExcelReference(this.Caller.RowFirst + offset, this.Caller.RowFirst + offset + this.ItemCount - 1,
							this.Caller.ColumnFirst + column.Index, this.Caller.ColumnFirst + column.Index));
					}
					CellBuilder cellBuilder;
					if (ranges.Count == 1) {
						cellBuilder = new CellBuilder(ranges.First());
					} else {
						cellBuilder = new CellBuilder(new ExcelReference(ranges));
					}
					action(cellBuilder);
					cellBuilder.Apply();
				}
			});
			return this;
		}
		public ArrayFunctionBuilder FormatHeader(Action<CellBuilder> action) {
			this.Queue(() => {
				if (this.hasHeader) {
					var cellBuilder = new CellBuilder(Caller.RowFirst, Caller.RowFirst, Caller.ColumnFirst, Caller.ColumnFirst + columns.Count - 1);
					action(cellBuilder);
					cellBuilder.Apply();
				}
			});
			return this;
		}
		private void Build() {
			for (int i = 0; i < orderedColumnNames.Length; i++) {
				var name = orderedColumnNames[i];
				if (columns.TryGetValue(name, out var column)) {
					column.Index = i;
				} else {
					throw new InvalidOperationException($"{name} is not a valid column name");
				}
			}
			var remainingColumns = this.columns.Values
				.Where(x => !orderedColumnNames.Contains(x.Name))
				.OrderBy(x => x.Index).ThenBy(x => x.Name).ThenBy(x => x.Title)
				.ToArray();

			for (int j = 0; j < remainingColumns.Length; j++) {
				remainingColumns[j].Index = j + orderedColumnNames.Length;
			}
		}
	}
}