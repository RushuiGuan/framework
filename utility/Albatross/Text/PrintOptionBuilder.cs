using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.Text {
	public class PrintOptionBuilder<T> where T:PrintOption, new(){
		Action<T>? action;
		public PrintOptionBuilder<T> Set(Action<T> action) {
			this.action += action;
			return this;
		}
		public T Build() {
			var option = new T();
			action?.Invoke(option);
			option.FormatValue = this.Format;
			option.Properties = this.properties.ToArray();
			return option;
		}
		List<string> properties = new List<string>();
		Dictionary<string, Func<object?, object?, string>> formatters = new Dictionary<string, Func<object?, object?, string>>();
		Dictionary<string, Func<object?, object?, Task<string>>> asyncFormatters = new Dictionary<string, Func<object?, object?, Task<string>>>();

		public PrintOptionBuilder<T> Property(params string[] properties) {
			this.properties.AddRange(properties);
			return this;
		}

		public PrintOptionBuilder<T> Format(string property, string format)  {
			this.formatters[property] = (object? entity, object? value) => string.Format($"{{0}}:{format}", value);
			return this;
		}
		public PrintOptionBuilder<T> Format(string property, Func<object?, object?, string> format) {
			this.formatters[property] = format;
			return this;
		}
		public PrintOptionBuilder<T> AsyncFormat(string property, Func<object?, object?, Task<string>> format) {
			this.asyncFormatters[property] = format;
			return this;
		}

		Task<string> Format(object? entity, string property, object? value) {
			if (asyncFormatters.TryGetValue(property, out var asyncFormatter)) {
				return asyncFormatter(entity, value);
			} else if (formatters.TryGetValue(property, out var formatter)) {
				return Task.FromResult(formatter(entity, value));
			} else {
				return PrintOption.DefaultFormatValue(entity, property, value);
			}
		}
	}

	public static class  PrintOptionBuilderExtensions {
		
		public static PrintOptionBuilder<T> ColumnHeaderLineCharacter<T>(this PrintOptionBuilder<T> builder, char value) where T:PrintOption, new() {
			return builder.Set(option => option.ColumnHeaderLineCharacter = value);
		}
		public static PrintOptionBuilder<PrintTableOption> PrintHeader(this PrintOptionBuilder<PrintTableOption> builder, bool value = true) {
			return builder.Set(option => option.PrintHeader = value);
		}
		public static PrintOptionBuilder<PrintTableOption> ColumnHeader(this PrintOptionBuilder<PrintTableOption> builder, Func<string, string> value) {
			return builder.Set(option => option.GetColumnHeader = value);
		}
		public static PrintOptionBuilder<PrintPropertiesOption> ColumnHeader(this PrintOptionBuilder<PrintPropertiesOption> builder, Func<int, string?> value) {
			return builder.Set(option => option.GetColumnHeader = value);
		}
		public static PrintOptionBuilder<PrintPropertiesOption> RowHeader(this PrintOptionBuilder<PrintPropertiesOption> builder, Func<string, string> value) {
			return builder.Set(option => option.GetRowHeader = value);
		}
	}
}
