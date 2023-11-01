using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Albatross.Excel {
	public record ArrayFunctionColumn {
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
		bool showHeader;
		ExcelReference caller { get; }
		ExcelAction? postReturnActions;
		List<ArrayFunctionColumn> columns = new List<ArrayFunctionColumn>();
		public object[,] Result { get; private set; } = new object[0, 0];

		public ArrayFunctionBuilder(Type type, bool showHeader) {
			this.type = type;
			this.showHeader = showHeader;
			this.caller = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
		}
		public ArrayFunctionBuilder Queue(Action<ExcelReference> postReturnAction) {
			if (postReturnActions == null) {
				postReturnActions = () => postReturnAction(this.caller);
			} else {
				postReturnActions += () => postReturnAction(this.caller);
			}
			return this;
		}
		public ArrayFunctionBuilder ShowHeader(bool show) {
			this.showHeader = show;
			return this;
		}
		public ArrayFunctionBuilder RestoreSelection() {
			this.Queue(caller => this.caller.Select());
			return this;
		}

		public object[,] SetValue<T>(IEnumerable<T> values) {
			if (type.IsAssignableTo(typeof(T))) {
				var array = values.ToArray();
				int offset = 0;
				if (showHeader) { offset = 1; }
				Result = new object[array.Length + offset, this.columns.Count];
				if (showHeader) {
					for (int i = 0; i < columns.Count; i++) {
						Result[0, i] = columns[i].Title;
					}
				}
				for (int rowIndex = 0; rowIndex < array.Length; rowIndex++) {
					for (int columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
						var srcValue = array[rowIndex];
						var value = columns[columnIndex].Func(array[rowIndex]);
						Result[rowIndex + offset, columnIndex] = CellValue.Write(value);
					}
				}
				return Result;
			} else {
				throw new ArgumentException($"ArrayFunction value has to be assignable from type {type.FullName}");
			}
		}
		public ArrayFunctionBuilder BoldHeader() {
			if (showHeader) {
				this.Queue(caller => new CellBuilder(caller.RowFirst, caller.RowFirst, caller.ColumnFirst, caller.ColumnFirst + columns.Count - 1)
					.FontProperties(x => x.Bold()).Apply());
			}
			return this;
		}
		public void QueuePostReturnActions() => ExcelAsyncUtil.QueueAsMacro(this.postReturnActions);
		public void QueueDefaultPostReturnActions() => this.BoldHeader().RestoreSelection().QueuePostReturnActions();

		public ArrayFunctionBuilder AddColumn(string name, string? title, Func<object?, object?> func) {
			columns.Add(new ArrayFunctionColumn(name, title, func));
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
	}
}