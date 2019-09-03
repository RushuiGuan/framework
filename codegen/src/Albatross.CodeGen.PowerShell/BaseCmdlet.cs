using Microsoft.Extensions.DependencyInjection;
using System;
using System.Management.Automation;

namespace Albatross.CodeGen.PowerShell {
	public abstract class BaseCmdlet<T> : PSCmdlet where T:class{
		protected T EntryObject { get; private set; }
		protected ServiceProvider provider;
		protected abstract void RegisterContainer(IServiceCollection svc);

		protected override void BeginProcessing() {
			base.BeginProcessing();
			System.Environment.CurrentDirectory = this.SessionState.Path.CurrentFileSystemLocation.Path;
			ServiceCollection svc = new ServiceCollection();
			RegisterContainer(svc);
			provider = svc.BuildServiceProvider();
			EntryObject = provider.GetRequiredService<T>();
		}
		protected override void EndProcessing() {
			base.EndProcessing();
			provider.Dispose();
		}
	}
}
