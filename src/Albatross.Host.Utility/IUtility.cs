using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Host.Utility {
	public interface IUtility : IDisposable {
		int Run();
		TextWriter Out { get; }
		TextWriter Error { get; }
	}
	public interface IUtility<T> : IUtility{
		T Options{ get; }
	}
}
