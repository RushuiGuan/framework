using Albatross.CommandQuery;
using System.Threading.Tasks;

namespace Albatross.Test.CommandQuery {
	public class MyCommandHandler : ICommandHandler<MyCommand> {
		public Task Handle(MyCommand command) {
			throw new System.NotImplementedException();
		}

		public Task Handle(Command command) {
			throw new System.NotImplementedException();
		}
	}
}