using System;
using System.Collections.Generic;

namespace Albatross.Text.Table {
	public class TableOptionFactory {
		object sync = new object();
		Dictionary<Type, TableOptions> registration = new Dictionary<Type, TableOptions>();
		public TableOptions<T> Get<T>() {
			lock (sync) {
				if (registration.TryGetValue(typeof(T), out TableOptions? options)) {
					return (TableOptions<T>)options;
				} else {
					var newOptions = new TableOptionBuilder<T>().AddPropertiesByReflection().Build();
					Register<T>(newOptions);
					return newOptions;
				}
			}
		}
		public static TableOptionFactory Instance { get; } = new TableOptionFactory();
		public void Register<T>(TableOptions<T> options) {
			lock (sync) {
				registration[typeof(T)] = options;
			}
		}
	}
}
