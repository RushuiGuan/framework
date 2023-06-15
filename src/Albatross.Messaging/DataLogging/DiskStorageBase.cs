using Albatross.IO;
using Albatross.Messaging.Configurations;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace Albatross.Messaging.DataLogging {
	public class DiskStorageBase {
		protected readonly DiskStorageConfiguration config;
		protected readonly ILogger logger;
		protected Encoding utf8 = new UTF8Encoding(false);
		protected DateTime TimeStamp => DateTime.Now;
		protected string FilePattern => $"{config.FileName}_*.txt";

		public DiskStorageBase(DiskStorageConfiguration config, ILogger logger) {
			this.config = config;
			this.logger = logger;
			if(!Directory.Exists(config.WorkingDirectory)) {
				Directory.CreateDirectory(config.WorkingDirectory);
			}
		}
	}
}
