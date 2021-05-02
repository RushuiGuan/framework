using Albatross.CommandQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Test.CommandQuery {
	public class MyCommand : Command {
		public MyCommand(bool waitForCompletion) : base(waitForCompletion) {
		}
	}
}
