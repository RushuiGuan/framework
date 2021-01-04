using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.Hosting.Demo {
	public class RazorViewPathProvider : IFileProvider {
		public IDirectoryContents GetDirectoryContents(string subpath) {
			return new DirectoryContent();
		}

		public IFileInfo GetFileInfo(string subpath) {
			if (string.Equals("/views/home/my very special view.default-super.cshtml", subpath, StringComparison.InvariantCultureIgnoreCase)) {
				return new InMemoryFile();
			} else {
				return null;
			}
		}

		public IChangeToken Watch(string filter) {
			return new ChangedToken();
		}
	}
	public class ChangedToken : IChangeToken {
		public bool HasChanged => false;

		public bool ActiveChangeCallbacks => false;

		public IDisposable RegisterChangeCallback(Action<object> callback, object state) {
			return null;
		}
	}
	public class DirectoryContent : IDirectoryContents {
		public bool Exists => false;

		public IEnumerator<IFileInfo> GetEnumerator() {
			return ((IEnumerable<IFileInfo>)new IFileInfo[0]).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return new IFileInfo[0].GetEnumerator();
		}
	}
	public class InMemoryFile : IFileInfo {
		public bool Exists => true;

		public long Length => throw new NotImplementedException();

		public string PhysicalPath => null;

		public string Name => "my very special view.default-super";

		public DateTimeOffset LastModified => DateTime.Now;

		public bool IsDirectory => false;

		public string text = @"
@model Albatross.Hosting.Demo.Controllers.Input
<div>@Model.Now</div>
<div>@Model.Name</div>
<div>@Model.Id</div>";

		public Stream CreateReadStream() {
			return new MemoryStream(UTF8Encoding.UTF8.GetBytes(text));
		}
	}
}
