using Microsoft.CodeAnalysis;

namespace Albatross.CodeAnalysis.Symbols {
	public interface ICompilationFactory {
		Compilation Create();
	}
}