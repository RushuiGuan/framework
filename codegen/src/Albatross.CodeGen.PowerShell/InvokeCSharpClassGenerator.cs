using System.IO;
using System;
using System.Management.Automation;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.CodeGen.PowerShell {
	[Cmdlet(VerbsLifecycle.Invoke, "CSharpClassGenerator")]
	public class InvokeCSharpClassGenerator : BaseCmdlet<Albatross.CodeGen.CSharp.Writer.WriteCSharpClass> {
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
		[Alias("c")]
		public Albatross.CodeGen.CSharp.Model.Class Class { get; set; }

		[Parameter(Position = 1, Mandatory = false)]
		[Alias("t")]
		public FileInfo Output { get; set; }

		[Parameter]
		public SwitchParameter Force { get; set; }

		StreamWriter writer;
		protected override void BeginProcessing() {
			base.BeginProcessing();
			if (Output != null) {
				if (!Output.Exists || Force || this.ShouldContinue("The file already exists, continue and overwrite?", "Warning")) {
					writer = new StreamWriter(new FileStream(Output.FullName, FileMode.OpenOrCreate));
				}
			}
		}
		protected override void ProcessRecord() {
			if (writer != null) {
				EntryObject.Run(writer, Class);
				writer.WriteLine();
			}
			EntryObject.Run(Console.Out, Class);
			Console.WriteLine();
		}
		protected override void EndProcessing() {
			if (writer != null) {
				writer.Flush();
				writer.BaseStream.SetLength(writer.BaseStream.Position);
				writer.Close();
			}

			base.EndProcessing();
		}

		protected override void RegisterContainer(IServiceCollection svc) {
			svc.AddDefaultCodeGen();
		}
	}
}
