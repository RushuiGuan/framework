using ExcelDna.Integration.CustomUI;
using ExcelDna.Integration.Extensibility;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Hosting.Excel {
	public class HostedExcelRibbon : ExcelRibbon {
		private readonly ILogger logger;

		public HostedExcelRibbon(ILogger logger) {
			this.logger = logger;
		}
		public override void OnConnection(object Application, ext_ConnectMode ConnectMode, object AddInInst, ref Array custom) {
			base.OnConnection(Application, ConnectMode, AddInInst, ref custom);
			logger.LogInformation("OnConnection");
		}
		public override void OnDisconnection(ext_DisconnectMode RemoveMode, ref Array custom) {
			base.OnDisconnection(RemoveMode, ref custom);
			logger.LogInformation("OnDisconnection");
		}
		public override void OnAddInsUpdate(ref Array custom) {
			base.OnAddInsUpdate(ref custom);
			logger.LogInformation("OnAddInsUpdate");
		}
		public override void OnStartupComplete(ref Array custom) {
			base.OnStartupComplete(ref custom);
			logger.LogInformation("OnStartupComplete");
		}
		public override void OnBeginShutdown(ref Array custom) {
			base.OnBeginShutdown(ref custom);
			logger.LogInformation("OnBeginShutdown");
		}
	}
}