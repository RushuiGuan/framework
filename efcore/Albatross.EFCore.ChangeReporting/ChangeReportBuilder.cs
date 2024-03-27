using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.EFCore.ChangeReporting {
	public class ChangeReportBuilder<T> where T : class {
		Action<ChangeReportDbEventHandler<T>>? action;

		public ChangeReportBuilder() {
			this.ExcludeAuditProperties();
			this.ExcludeTemporalProperties();
		}

		public ChangeReportBuilder<T> Set(Action<ChangeReportDbEventHandler<T>> action) {
			this.action += action;
			return this;
		}

		public ChangeReportDbEventHandler<T> Build() {
			var handler = new ChangeReportDbEventHandler<T>();
			this.action?.Invoke(handler);
			if(handler.OnReportGenerated == null) {
				throw new InvalidOperationException($"{typeof(T).Name} ChangeReportDbEventHandler is missing OnReportGenerated delegate");
			}
			return handler;
		}
	}
}
