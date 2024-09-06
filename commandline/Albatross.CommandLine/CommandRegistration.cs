using Microsoft.Extensions.DependencyInjection;

namespace Albatross.CommandLine {
	public interface ICommandRegistration {
		string Command { get; }
		IServiceCollection RegisterServices(IServiceCollection services);
	}
}
