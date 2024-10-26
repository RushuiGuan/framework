using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using System.Threading.Tasks;

namespace Sample.CommandHandlers {
	public class SelfDestructCommandHandler : BaseCommandHandler<SelfDestructCommand> {
		private readonly ILogger<SelfDestructCommandHandler> logger;

		public SelfDestructCommandHandler(ILogger<SelfDestructCommandHandler> logger) {
			this.logger = logger;
		}

		public override async Task Handle(SelfDestructCommand command) {
			var delay = command.Delay ?? 3;
			logger.LogInformation("Self destructing in {delay} second", delay);
			await Task.Delay(delay * 1000);
			System.Diagnostics.Process.GetCurrentProcess().Kill();
		}
	}
}