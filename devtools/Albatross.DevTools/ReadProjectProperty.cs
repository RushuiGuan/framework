using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.DevTools {
	[Verb("read-project-property", typeof(ReadProjectProperty))]
	public class ReadProjectPropertyOptions {
		[Option("f")]
		public FileInfo ProjectFile { get; set; } = null!;
		[Option("p")]
		public string Property { get; set; } = string.Empty;
	}
	public class ReadProjectProperty : BaseHandler<ReadProjectPropertyOptions> {
		public ReadProjectProperty(IOptions<ReadProjectPropertyOptions> options, ILogger logger) : base(options, logger) {
		}
		public override Task<int> InvokeAsync(InvocationContext context) {
			var doc = new System.Xml.XmlDocument();
			doc.Load(this.options.ProjectFile.FullName);
			var result = doc.SelectSingleNode($"/Project/PropertyGroup/{options.Property}")?.InnerText;
			if (!string.IsNullOrEmpty(result)) {
				writer.WriteLine(result);
				return Task.FromResult(0);
			} else {
				return Task.FromResult(1);
			}
		}
	}
}