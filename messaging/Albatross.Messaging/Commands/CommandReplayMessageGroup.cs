using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Commands {
	public class CommandReplayMessageGroup {
		public int Index { get; set; }
		public CommandRequest Request { get; init; }
		public IMessage? Response { get; set; }
		public ClientAck? Acked { get; set; }

		public CommandReplayMessageGroup(CommandRequest request, int index) {
			this.Request = request;
			this.Index = index;
		}

		public bool IsCompleted {
			get {
				if (Request.Mode == CommandMode.Callback) {
					return Response != null && Acked != null;
				} else {
					return Response != null;
				}
			}
		}
	}
}