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

		public IDbSession Build(IDbSession session) => Build(session, new ChangeReportDbSessionEventHandler<T>());
		public IDbSession Build(IDbSession session, ChangeReportDbSessionEventHandler<T> handler) {
			this.action?.Invoke(handler);
			session.SessionEventHandlers.Add(handler);
			return session;
		}
	}
}
