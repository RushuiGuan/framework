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
		public FileInfo MarkdownFile { get; set; } = null!;
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
		public static string GetRelativeUrl(DirectoryInfo rootFolder, FileInfo file) {
			var result = Path.GetRelativePath(rootFolder.FullName, file.FullName);
			if(Path.IsPathRooted(result)) {
				throw new ArgumentException($"Cannot create relative path from {file.FullName} to {rootFolder}");
			}
			return result;
		}
		public FixMarkDownRelativeUrls(IOptions<FixMarkDownRelativeUrlsOptions> options) : base(options) { }
		static Regex regex = new Regex(@"\[(.*?)\]\((.*?)\)", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

		public static string ReplaceAll(string text, DirectoryInfo rootFolder, FileInfo markdownFile, string rootUrl) {
			return regex.Replace(text, (match) => {
				var path = match.Groups[2].Value;
				if (new Uri(path, UriKind.RelativeOrAbsolute).IsAbsoluteUri) {
					return match.Value;
				} else {
					var relativePath = GetRelativeUrl(rootFolder, markdownFile);
					var newUrl = GetAbsoluteUrl(rootUrl, relativePath, path);
					return $"[{match.Groups[1].Value}]({newUrl})";
				}
			});
		}

		public override async Task<int> InvokeAsync(InvocationContext context) {
			if (!this.options.MarkdownFile.Exists) {
				throw new InvalidOperationException($"File {this.options.MarkdownFile.FullName} does not exist");
			}
			var text = await File.ReadAllTextAsync(this.options.MarkdownFile.FullName);
			var newText = ReplaceAll(text, this.options.RootFolder, this.options.MarkdownFile, this.options.RootUrl);
			await File.WriteAllTextAsync(this.options.MarkdownFile.FullName, newText);
			return 0;
		}
	}
}
