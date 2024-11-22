using Albatross.CommandLine;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.CommandLine {
	[Verb("search", typeof(HelpCommandHandler))]
	public record class SearchOptions { }

	[Verb("id", typeof(SearchCommandHandler), Parent = "search")]
	public class SearchByIdOptions {
		public int Id { get; set; }
	}
	[Verb("name", typeof(SearchCommandHandler), Parent = "search")]
	public class SearchByNameOptions {
		public string Name { get; set; } = string.Empty;
	}
	public class SearchCommandHandler : ICommandHandler {
		public SearchCommandHandler(IOptions<SearchByIdOptions>? searchByIdOptions, IOptions<SearchByNameOptions>? searchByNameOptions) {
		}

		public int Invoke(InvocationContext context) {
			// do something
			return 0;
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			// do something
			return Task.FromResult(0);
		}
	}
}
