using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Albatross.DevTools {
	[Verb("format-xml", typeof(FormatXml), Description = "Format the xml file")]
	public class FormatXmlOptions {
		[Option("f")]
		public FileInfo XmlFile { get; set; } = null!;
		[Option("i", Required = false, Description = "If 0, tab is used for indent.  Otherwise the specified number of spaces are used instead")]
		public int SpaceIndentSize { get; set; }
	}
	public class FormatXml : BaseHandler<FormatXmlOptions> {
		public FormatXml(IOptions<FormatXmlOptions> options, ILogger logger) : base(options, logger) {
		}
		public override Task<int> InvokeAsync(InvocationContext context) {
			if (!this.options.XmlFile.Exists) {
				throw new InvalidOperationException($"File {this.options.XmlFile.FullName} does not exist");
			}
			var doc = new System.Xml.XmlDocument();
			doc.Load(this.options.XmlFile.FullName);
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			if (options.SpaceIndentSize == 0) {
				settings.IndentChars = "\t";
			} else {
				settings.IndentChars = new string(' ', this.options.SpaceIndentSize);
			}
			using(var writer = new StreamWriter(options.XmlFile.FullName)) {
				using (XmlWriter xml = XmlWriter.Create(writer, settings)) {
					doc.Save(xml);
				}
			}
			return Task.FromResult(0);
		}
	}
}
