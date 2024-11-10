using Albatross.CommandLine;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.DevTools {
	[Verb("git-hash", typeof(GetGitHash))]
	public record class GitOptions{
		[Option("d")]
		public DirectoryInfo GitDirectory { get; set; } = null!;

		[Option("v")]
		public string Version { get; set; } = string.Empty;
	}
	public class GetGitHash : BaseHandler<GitOptions> {
		public GetGitHash(IOptions<GitOptions> options, ILogger logger) : base(options, logger) {
		}
		public override Task<int> InvokeAsync(InvocationContext context) {


			using var repo = new Repository(options.GitDirectory.FullName);
			this.writer.WriteLine(repo.Commits.First().Id);
			this.writer.WriteLine(repo.Commits.First().Sha);
			this.writer.WriteLine(repo.Commits.Count());
			this.writer.WriteLine(repo.Branches.First().FriendlyName);
			return Task.FromResult(0);
		}
	}
}
