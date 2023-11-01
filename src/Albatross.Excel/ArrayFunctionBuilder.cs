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

		public ArrayFunctionBuilder AddColumn(string name, string? title, Func<object?, object?> func) {
			columns.Add(name, new ArrayFunctionColumn(name, title, func) {
				Index = columns.Count,
			});
			return this;
		}
		public ArrayFunctionBuilder AddColumnsByReflection(params string[] fields) {
			if (fields.Length != 0) {
				foreach (var field in fields) {
					var info = this.type.GetProperty(field, BindingFlags.Public | BindingFlags.Instance)
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
		public ArrayFunctionBuilder SetOrder(string name, int order) {
			if(columns.TryGetValue(name, out var column)) {
				column.Index = order;
				return this;
			} else {
				throw new ArgumentException($"{name} is not an existing column");
			}
		}
		public ArrayFunctionBuilder FormatColumns(Action<CellBuilder> action, params string[] items) {
			var selected = new List<ArrayFunctionColumn>();
			foreach (var item in items) {
				if (columns.TryGetValue(item, out var column)) {
					selected.Add(column);
				} else {
					throw new ArgumentException($"{item} is not an existing column");
				}
			}
			this.Queue(() => {
				var ranges = new List<ExcelReference>();
				var offset = this.hasHeader ? 1 : 0;
				foreach (var column in selected) {
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
			var items = this.columns.Values.OrderBy(x => x.Index).ThenBy(x => x.Name).ThenBy(x => x.Title).ToArray();
			for (int i = 0; i < items.Length; i++) {
				items[i].Index = i;
			}
		}
	}
}