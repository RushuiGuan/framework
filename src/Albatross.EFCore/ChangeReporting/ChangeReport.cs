namespace Albatross.EFCore.ChangeReporting {
	public class ChangeReport<T> : IChangeReport where T : class {
		public T Entity { get; set; }
		public ChangeType Type { get; set; }
		public ChangeReport(T entity, string property) {
			Entity = entity;
			Property = property;
		}
		public string Property { get; set; }
		public object? OriginalValue { get; set; }
		public object? CurrentValue { get; set; }
		object IChangeReport.Entity => Entity;
	}
}