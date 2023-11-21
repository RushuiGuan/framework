using System;
using System.IO;
using System.Reflection;

namespace Albatross.Messaging.Configurations {
	public class DiskStorageConfiguration {
		public string WorkingDirectory { get; set; }
		internal string FileName { get; set; }
		/// <summary>
		/// default max file size is 50MB
		/// </summary>
		public long MaxFileSize { get; set; }

		string DefaultWorkingDirectory() {
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			var programName = Assembly.GetEntryAssembly()?.GetName().Name ?? throw new InvalidOperationException("The execution assembly not found or is missing name");
			return Path.Join(folder, programName);
		}

		public DiskStorageConfiguration(string? workingDirectory, string filename) {
			WorkingDirectory = workingDirectory ?? DefaultWorkingDirectory();
			FileName = filename;
			MaxFileSize = 1024L * 1024L * 50;
		}
	}
}