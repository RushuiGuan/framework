namespace Albatross.EFCore.ChangeReporting {
	public interface IChangeReport {
		public object Entity { get; }
		public ChangeType Type { get; }
		public string Property { get; }
		public object? OriginalValue { get; }
		public object? CurrentValue { get; }
	}

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