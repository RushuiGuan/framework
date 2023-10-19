using Albatross.Messaging.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Commands {
	[Command]
	public class KickOffDoNothingCommand {
		public int Id { get; set; }
	}
}
