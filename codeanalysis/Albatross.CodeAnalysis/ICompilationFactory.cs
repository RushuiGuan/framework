using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Albatross.CodeAnalysis {
	public interface ICompilationFactory {
		Task<Compilation> Get();
	}
}
