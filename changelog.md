* v4.1.0
	* upgrade all dependencies to latest release
		* csvhelper major upgrade 28 to 29
		* azure.storage.blobs minor upgrade 12.13 to 12.14
		* serilog minor upgrade 2.11 to 2.12
		* microsoft test sdk 17.2 to 17.3
		* all other upgrades are patch
	* upgrade to efcore 7 rc
	* remove Albatross.Repository.ByEfCore.Repository class because it is never used

* v4.0.2
	* Albatross.WebClient
		* **breaking:** renaming Invoke methods to multiple GetResponse methods so that its intent is more clear
		* Albatross.CodeGen.WebClient has been updated accordingly
		* Albatross.WebClient.ClientBase.CreateRequests method has been deprecated in favor of CreateRequestUrls.  Since requests cannot be reused, it is necessary to create new requests when using polly retry mechanism.  CreateRequestUrls make it easier to create new requests and a much better alternative than cloning the existing request.
		* **breaking** remove a bunch of extension methods in Albatross.WebClient.Extension static class.  Users are encouraged to use CreateRequest adn GetResponse methods for consistency.
	* Albatross.CodeGen.WebClient
		* a bit of code clean up in C# web client proxy generation
	* add test code for polly in webclient
	* Albatross.Math
		* trim trailing zeros of a decimal number when converting to double
			* https://stackoverflow.com/questions/1584314/conversion-of-a-decimal-to-double-number-in-c-sharp-results-in-a-difference
			
