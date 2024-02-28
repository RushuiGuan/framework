using System;

namespace Albatross.EFCore.ChangeReporting {
	public class ChangeReportBuilder<T> where T : class {
		Action<ChangeReportDbSessionEventHandler<T>>? action;

		public ChangeReportBuilder<T> Set(Action<ChangeReportDbSessionEventHandler<T>> action) {
			this.action += action;
			return this;
		}

		public ChangeReportDbSessionEventHandler<T> Build() => Build(new ChangeReportDbSessionEventHandler<T>());
		public ChangeReportDbSessionEventHandler<T> Build(ChangeReportDbSessionEventHandler<T> handler) {
			this.action?.Invoke(handler);
			return handler;
		}
	}
}
