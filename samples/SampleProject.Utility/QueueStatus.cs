using CommandLine;
using System.Threading.Tasks;
using SampleProject.Proxy;

namespace SampleProject.Utility {
	[Verb("queue-status")]
	public class QueueStatusOption : MyBaseOption{	}

	public class QueueStatus: MyUtilityBase<QueueStatusOption> {
		public QueueStatus(QueueStatusOption option) : base(option) {
		}

		public async Task<int> RunUtility(RunProxyService svc) {
			var result = await svc.QueueStatus();
			Options.WriteOutput(result);
			return 0;
		}
	}
}
