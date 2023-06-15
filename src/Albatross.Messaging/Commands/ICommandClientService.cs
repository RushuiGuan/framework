using Albatross.Messaging.Services;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	/// <summary>
	/// With the exception of the Start and Dispose method, which is used for initialization, 
	/// all other methods in this call should be thread safe.
	/// </summary>
	public interface ICommandClientService : IDealerClientService {
		Task<ResponseType> Submit<CommandType, ResponseType>(CommandType command)
			where CommandType : Command<ResponseType>
			where ResponseType : notnull;
		Task Submit<CommandType>(CommandType command, bool fireAndForget) where CommandType : Command;
		Task<CommandQueueInfo[]> QueueStatus();
		Task Ping();
	}
}
