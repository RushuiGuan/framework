using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Albatross.DevTools {
	[Verb("fix-markdown-relative-urls", typeof(FixMarkDownRelativeUrls), Description = Description)]
	public record class FixMarkDownRelativeUrlsOptions {
		const string Description = "Replace the relative urls in a markdown file with absolute urls using the provided RootUrl and RootFolder.  The new url will be the constructed with the format of: {RootUrl}/{PathRelativeToRootFolder}.  The utility is useful to fix the relative urls in the README.md file since the file is packed as part of the nuget package and its relative urls will not work in the nuget.org website";
		public FileInfo MarkDownFile { get; set; } = null!;
		public string RootUrl { get; set; } = string.Empty;
		public DirectoryInfo RootFolder { get; set; } = null!;
	}
	public class FixMarkDownRelativeUrls : BaseHandler<FixMarkDownRelativeUrlsOptions> {
		public static string GetAbsoluteUrl(string rootUrl, string relativeFilePath, string relativeUrl) {
			if (Path.IsPathRooted(relativeFilePath)) {
				throw new ArgumentException($"{relativeFilePath} must be a relative path to the root url");
			}
			var uri = new Uri(new Uri(rootUrl), relativeFilePath);
			try {
				new Uri(relativeUrl, UriKind.Relative);
			} catch(UriFormatException) {
				throw new ArgumentException($"{relativeUrl} is not a relative url");
			}
			uri = new Uri(uri, relativeUrl);
			return uri.AbsoluteUri;
		}
		public static string GetRelativeUrl(string rootFolder, FileInfo file) {
			if(!Path.IsPathRooted(rootFolder)) {
				throw new ArgumentException($"{rootFolder} must be an absolute path");
			}
			var result = Path.GetRelativePath(rootFolder, file.FullName);
			if(Path.IsPathRooted(result)) {
				throw new ArgumentException($"Cannot create relative path from {file.FullName} to {rootFolder}");
			}
			return result;
		}
		public FixMarkDownRelativeUrls(IOptions<FixMarkDownRelativeUrlsOptions> options, ILogger logger) : base(options, logger) { }
		static Regex regex = new Regex(@"\[(.*?)\]\((.*?)\)", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
		public override async Task<int> InvokeAsync(InvocationContext context) {
			if (!this.options.MarkDownFile.Exists) {
				throw new InvalidOperationException($"File {this.options.MarkDownFile.FullName} does not exist");
			}
			var text = await File.ReadAllTextAsync(this.options.MarkDownFile.FullName);
			var newText = regex.Replace(text, (match) => {
				var path = match.Groups[2].Value;
				if (new Uri(path).IsAbsoluteUri) {
					return match.Value;
				} else {
					var relativePath = GetRelativeUrl(this.options.RootFolder.FullName, this.options.MarkDownFile);
					var newUrl = GetAbsoluteUrl(this.options.RootFolder.FullName, relativePath, path);
					return match.Value.Replace(path, newUrl);
				}
			});
			return 0;
		}
	}
}
