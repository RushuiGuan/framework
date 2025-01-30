using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.DevTools {
	[Verb("project-list", typeof(GetProjectList))]
	public class GetProjectListOptions {
		[Option("f")]
		public FileInfo File { get; set; } = null!;
		[Option("h")]
		public string Header { get; set; } = string.Empty;
	}
	public class GetProjectList : BaseHandler<GetProjectListOptions> {
		public GetProjectList(IOptions<GetProjectListOptions> options) : base(options) {
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			string? current = null;
			using var reader = new StreamReader(this.options.File.FullName);
			while (!reader.EndOfStream) {
				var line = await reader.ReadLineAsync();
				if (!string.IsNullOrEmpty(line)) {
					if (IsHeader(line, out string? header)) {
						current = header;
					} else if (string.Equals(current, options.Header, System.StringComparison.InvariantCultureIgnoreCase)) {
						this.writer.WriteLine(line.Trim());
					}
				}
			}
			return 0;
		}

		bool IsHeader(string line, [NotNullWhen(true)] out string? header) {
			line = line.Trim();
			if (line.StartsWith("#")) {
				header = line.Substring(1).Trim();
				return true;
			}
			header = null;
			return false;
		}
	}
}
