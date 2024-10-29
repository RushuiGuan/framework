#if NETSTANDARD2_1
using System.ComponentModel;
namespace System.Runtime.CompilerServices {
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static class IsExternalInit {
	}
}
#endif