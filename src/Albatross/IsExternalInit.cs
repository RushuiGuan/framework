/// this is left here to allow use of new keywords such as record and init;
namespace System.Runtime.CompilerServices {
	using System.ComponentModel;
	/// <summary>
	/// Reserved to be used by the compiler for tracking metadata.
	/// This class should not be used by developers in source code.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static class IsExternalInit {
	}
}
