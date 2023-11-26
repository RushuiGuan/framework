using Azure.Storage.Blobs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.AzureStorage {
	public static class Extensions {
		public static async Task<BlobClient> Move(this BlobClient src, BlobContainerClient container, string file, bool overwrite, CancellationToken cancellationToken = default) {
			var dst = container.GetBlobClient(file);
			if(src.Name == dst.Name) {
				return src;
			}
			var existing = await dst.ExistsAsync(cancellationToken);
			if (existing && !overwrite) {
				throw new InvalidOperationException($"cannot overwrite existing blob {file} without setting the overwrite flag");
			}
			DateTimeOffset expiredOn = DateTimeOffset.UtcNow.AddMinutes(60);
			var sasUri = src.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, expiredOn);
			var result = await dst.StartCopyFromUriAsync(sasUri);

			while (!result.HasCompleted) {
				await result.WaitForCompletionAsync();
			}
			await src.DeleteIfExistsAsync();
			return dst;
		}
	}
}
