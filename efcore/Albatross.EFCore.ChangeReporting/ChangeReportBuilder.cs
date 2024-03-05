using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.EFCore.ChangeReporting {
	public class ChangeReportBuilder<T> where T : class {
		Action<ChangeReportDbSessionEventHandler<T>>? action;

		public ChangeReportBuilder() {
			this.ExcludeAuditProperties();
			this.ExcludeTemporalProperties();
		}

		public ChangeReportBuilder<T> Set(Action<ChangeReportDbSessionEventHandler<T>> action) {
			this.action += action;
			return this;
		}

		public ChangeReportDbSessionEventHandler<T> Build() {
			var handler = new ChangeReportDbSessionEventHandler<T>();
			this.action?.Invoke(handler);
			return handler;
		}
	}
}
