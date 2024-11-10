using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Albatross.DevTools {
	[Verb("remove-project-version", typeof(RemoveProjectVersion))]
	public record class RemoveProjectVersionOptions {
		[Option("d")]
		public DirectoryInfo Directory { get; set; } = null!;
	}
	public class RemoveProjectVersion : BaseHandler<RemoveProjectVersionOptions> {
		public RemoveProjectVersion(IOptions<RemoveProjectVersionOptions> options, ILogger logger) : base(options, logger) {
		}
		public override Task<int> InvokeAsync(InvocationContext context) {
			var projectFiles = Directory.GetFiles(this.options.Directory.FullName, "*.csproj", SearchOption.AllDirectories);
			foreach(var projectFile in projectFiles) {
				logger.LogInformation("Remove version for {projectFile}", projectFile);
				var doc = new XmlDocument();
				doc.Load(projectFile);
				if (RemoveVersion(doc)) {
					doc.Save(projectFile);
				}
			}
			return Task.FromResult(0);
		}
		bool RemoveVersion(XmlDocument projectXml) {
			XmlElement? versionNode = projectXml.SelectSingleNode("/Project/PropertyGroup/Version") as XmlElement;
			if(versionNode != null && versionNode.ParentNode != null) {
				versionNode.ParentNode.RemoveChild(versionNode);
				return true;
			}else {
				return false;
			}
		}
	}
}
