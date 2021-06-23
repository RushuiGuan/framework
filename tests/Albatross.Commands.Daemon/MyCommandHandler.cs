using System;
using System.Threading.Tasks;

namespace Albatross.Commands.Daemon {
	public class MyCommandHandler : BaseCommandHandler<MyCommand, int> {
		private readonly IEventPublisher<MyCommandExecuted> eventPublisher;

		public MyCommandHandler(IEventPublisher<MyCommandExecuted> eventPublisher) {
			this.eventPublisher = eventPublisher;
		}

		public override async Task<int> Handle(MyCommand command) {
			if (command.Fail) {
				throw new System.Exception("This is destine to fail");
			}
			await this.eventPublisher.Send(new MyCommandExecuted(Guid.NewGuid().ToString()));
			return 0;
		}
	}
}
