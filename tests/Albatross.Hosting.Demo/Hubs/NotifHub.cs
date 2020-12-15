using Microsoft.AspNetCore.Authorization;

namespace Albatross.Hosting.Demo.Hubs {
	[Authorize]
	public class NotifHub : Microsoft.AspNetCore.SignalR.Hub {
	}
}
