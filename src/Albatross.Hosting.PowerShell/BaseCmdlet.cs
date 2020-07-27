using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace Albatross.Hosting.PowerShell {
	public abstract class BaseCmdlet<T> : PSCmdlet where T:Session{
		protected T Session { get; private set; }
		protected abstract string SessionVariableName { get; }

		protected override void BeginProcessing() {
			base.BeginProcessing();
			this.Session = base.SessionState.PSVariable.GetValue(SessionVariableName) as T;
			if(Session == null){
				throw new Exception($"Session ${typeof(T).FullName} not initialized");
			}
		}
	}
}
