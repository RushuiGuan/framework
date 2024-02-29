namespace Albatross.EFCore.ChangeReporting {
	public interface IChangeReport {
		public object Entity { get; }
		public ChangeType Type { get; }
		public string Property { get; }
		public object? OldValue { get; }
		public object? NewValue { get; }
	}
}