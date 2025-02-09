namespace Albatross.TextGrid {
	public class TableBuilderFactory {
		object sync = new object();
		Dictionary<Type, TableOptions> registration = new Dictionary<Type, TableOptions>();
		public TableOptions<T> Get<T>() {
			lock (sync) {
				if (registration.TryGetValue(typeof(T), out TableOptions? options)) {
					return (TableOptions<T>)options;
				} else {
					return Register<T>(new TableBuilder<T>());
				}
			}
		}
		public static TableBuilderFactory Instance { get; } = new TableBuilderFactory();
		public TableOptions<T> Register<T>(TableBuilder<T> builder) {
			var options = new TableOptions<T>(builder);
			lock (sync) {
				registration[typeof(T)] = options;
			}
			return options;
		}
	}
}
