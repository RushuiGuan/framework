using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Albatross.DevTools {
	[Verb("set-project-version", typeof(SetProjectVersion))]
	public record class SetProjectVersionOptions {
		[Option("d")]
		public DirectoryInfo Directory { get; set; } = null!;
		[Option("ver")]
		public string Version { get; set; } = string.Empty;
	}
	public class SetProjectVersion : BaseHandler<SetProjectVersionOptions> {
		private readonly ILogger<SetProjectVersion> logger;

		public SetProjectVersion(IOptions<SetProjectVersionOptions> options, ILogger<SetProjectVersion> logger) : base(options) {
			this.logger = logger;
		}
		public override Task<int> InvokeAsync(InvocationContext context) {
			var projectFiles = Directory.GetFiles(this.options.Directory.FullName, "*.csproj", SearchOption.AllDirectories);
			foreach(var projectFile in projectFiles) {
				logger.LogInformation("Setting version for {projectFile}", projectFile);
				var doc = new XmlDocument();
				doc.Load(projectFile);
				SetVersion(doc);
				doc.Save(projectFile);
			}
			return Task.FromResult(0);
		}
		void SetVersion(XmlDocument projectXml) {
			XmlElement? versionNode = projectXml.SelectSingleNode("/Project/PropertyGroup/Version") as XmlElement;
			if(versionNode == null) {
				versionNode = projectXml.CreateElement("Version");
				var propertyGroupNode = projectXml.SelectSingleNode("/Project/PropertyGroup");
				if (propertyGroupNode == null) {
					propertyGroupNode = projectXml.CreateElement("PropertyGroup");
					var projectNode = projectXml.SelectSingleNode("/Project");
					if (projectNode == null) {
						throw new System.Exception("Project node not found");
					} else {
						projectNode.AppendChild(propertyGroupNode);
					}
				} else {
					propertyGroupNode.AppendChild(versionNode);
				}
			}
			versionNode.InnerText = this.options.Version;
		}
	}
}
