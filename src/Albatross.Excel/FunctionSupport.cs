using ExcelDna.Integration;
using System;

namespace Albatross.Excel {
	public class FunctionSupport {
		ExcelReference caller { get; }
		ExcelAction? actions;

		public FunctionSupport() {
			this.caller = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
		}


		public FunctionSupport Queue(Action<ExcelReference> action) {
			if(actions == null) {
				actions = () => action(this.caller);
			} else {
				actions += () => action(this.caller);
			}
			return this;
		}

		public FunctionSupport RestoreSelectionToCaller() {
			this.Queue(caller => this.caller.Select());
			return this;
		}

		public void Execute() {
			ExcelAsyncUtil.QueueAsMacro(this.actions);
		}
	}
}
