using Albatross.Reflection;
using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Albatross.Excel {
	public record ArrayFunctionColumn<T> {
		public string Name { get; set; }
		public string Title { get; set; }
		public Func<T, object?> Func { get; set; }
		public ArrayFunctionColumn(string name, string? title, Func<T, object?> func) {
			this.Name = name;
			this.Title = title ?? name;
			this.Func = func;
		}
	}
	public class ArrayFunctionBuilder<T> {
		ExcelReference caller { get; }
		ExcelAction? postReturnActions;
		List<ArrayFunctionColumn<T>> columns = new List<ArrayFunctionColumn<T>>();
		public object[,] Result { get; private set; } = new object[0, 0];
		
		public ArrayFunctionBuilder() {
			this.caller = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
		}
		public ArrayFunctionBuilder<T> Queue(Action<ExcelReference> postReturnAction) {
			if(postReturnActions == null) {
				postReturnActions = () => postReturnAction(this.caller);
			} else {
				postReturnActions += () => postReturnAction(this.caller);
			}
			return this;
		}
		public ArrayFunctionBuilder<T> RestoreSelection() {
			this.Queue(caller => this.caller.Select());
			return this;
		}
		public object[,] SetValue(IEnumerable<T> values) {
			var array = values.ToArray();
			Result = new object[array.Length + 1, this.columns.Count];
			for(int i=0; i<columns.Count; i++) {
				Result[0, i] = columns[i].Title;
			}
			for (int rowIndex = 0; rowIndex < array.Length; rowIndex++) {
				for (int columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
					var value = columns[columnIndex].Func(array[rowIndex]);
					Result[rowIndex + 1, columnIndex] = CellValue.Write(value);
				}
			}
			return Result;
		}
		public ArrayFunctionBuilder<T> BoldHeader() {
			this.Queue(caller => new CellBuilder(caller.RowFirst, caller.RowFirst, caller.ColumnFirst, caller.ColumnFirst + columns.Count -1)
				.FontProperties(x => x.Bold()).Apply());
			return this;
		}
		public void QueuePostReturnActions() => ExcelAsyncUtil.QueueAsMacro(this.postReturnActions);

		public ArrayFunctionBuilder<T> AddColumn(string name, string? title, Func<T, object?> func) {
			columns.Add(new ArrayFunctionColumn<T>(name, title, func));
			return this;
		}
		public ArrayFunctionBuilder<T> AddColumnsByReflection(params string[] fields) {
			var type = typeof(T);
			if (fields.Length != 0) {
				foreach (var field in fields) {
					var info = typeof(T).GetProperty(field, BindingFlags.Public | BindingFlags.Instance)
						?? throw new ArgumentException($"{field} is not a valid public, instance property for {type.Name}");
					AddColumn(info.Name, null, entity => info.GetValue(entity));
				}
			} else {
				foreach(var info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
					AddColumn(info.Name, null, entity => info.GetValue(entity));
				}
			}
			return this;
		}
	}
}