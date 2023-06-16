using Albatross.Messaging.Commands;
using System.Threading.Tasks;

namespace Albatross.Messaging.Services {
	/// <summary>
	/// With the exception of the Start and Dispose method, which is used for initialization, 
	/// all other methods in this call should be thread safe.
	/// </summary>
	public interface ICommandClient {
		Task<ResponseType> Submit<CommandType, ResponseType>(CommandType command)
			where CommandType : notnull where ResponseType : notnull;

		Task Submit<CommandType>(CommandType command, bool fireAndForget = true) where CommandType : notnull;
		Task<CommandQueueInfo[]> QueueStatus();
		Task Ping();
	}
}