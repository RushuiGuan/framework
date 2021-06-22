using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.Commands {
	/// <summary>
	/// command queue should be registered as transient.  Its instances are managed by the CommandBus
	/// </summary>
	public interface ICommandQueue: IDisposable {
		void Submit(Command command);
		Task Start();
		void Signal();
		Command? Last { get; }
		CommandQueueDto CreateDto();
	}
}