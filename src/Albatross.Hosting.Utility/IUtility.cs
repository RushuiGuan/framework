using System;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Hosting.Utility {
	public interface IUtility : IDisposable {
		Task<int> Run();
		TextWriter Out { get; }
		TextWriter Error { get; }
	}
	public interface IUtility<T> : IUtility{
		T Options{ get; }
	}
}
