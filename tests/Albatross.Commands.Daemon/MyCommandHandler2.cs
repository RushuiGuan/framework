using System;
using System.Threading.Tasks;

namespace Albatross.Commands.Daemon {
	public class MyCommandHandler2 : BaseCommandHandler<MyCommand2, int> {
		private readonly IEventPublisher<MyCommandExecuted> eventPublisher;

		public MyCommandHandler2(IEventPublisher<MyCommandExecuted> eventPublisher) {
			this.eventPublisher = eventPublisher;
		}

		public override async Task<int> Handle(MyCommand2 command) {
			if (command.Fail) {
				throw new System.Exception("This is destine to fail");
			}
			await this.eventPublisher.Send(new MyCommandExecuted(Guid.NewGuid().ToString()));
			return 0;
		}
	}
}
