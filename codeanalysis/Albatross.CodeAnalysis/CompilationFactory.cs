using Microsoft.CodeAnalysis;

namespace Albatross.CodeAnalysis {
	public interface ICompilationFactory {
		Compilation Create();
	}
}