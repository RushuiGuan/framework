using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace Albatross.Hosting.PowerShell {


	public class StartTestSession : PSCmdlet {
		protected override void ProcessRecord() {
			Session session = new Session();
			this.SessionState.PSVariable.Set(new PSVariable("session", session, ScopedItemOptions.AllScope));
		}
	}
}
