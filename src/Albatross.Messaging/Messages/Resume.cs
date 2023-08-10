using NetMQ;

namespace Albatross.Messaging.Messages {
	/// <summary>
	/// When the router server receives a heartbeat from a dead client.  It sends the Resume message for this client to 
	/// its <see cref="Albatross.Messaging.Services.IRouterServerService"/>
	/// </summary>
	public record class Resume : Message, IMessage, ISystemMessage {
		public static string MessageHeader { get => "resume"; }
		public Resume(string route, ulong id) : base(MessageHeader, route, id) { }
		public Resume() { }
	}
}