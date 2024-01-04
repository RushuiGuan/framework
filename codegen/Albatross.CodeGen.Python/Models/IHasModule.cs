using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Python.Models {
	public interface IHasModule {
		public string Module { get; }
		public string Name { get; }	
	}
}
