namespace Albatross.Text.Table {
	public class TableOptionFactory {
		object sync = new object();
		Dictionary<Type, TableOptions> registration = new Dictionary<Type, TableOptions>();
		public TableOptions<T> Get<T>() {
			lock (sync) {
				if (registration.TryGetValue(typeof(T), out TableOptions? options)) {
					return (TableOptions<T>)options;
				} else {
					return Register<T>(new TableOptionBuilder<T>());
				}
			}
		}
		public static TableOptionFactory Instance { get; } = new TableOptionFactory();
		public TableOptions<T> Register<T>(TableOptionBuilder<T> builder) {
			var options = new TableOptions<T>(builder);
			lock (sync) {
				registration[typeof(T)] = options;
			}
			return options;
		}
	}
}
