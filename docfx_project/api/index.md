### Release Notes
* v5.1.0
	* secured Albatross prefix in nuget.org
	* create a prod release prior the inclusion of messaging capability
	* web client improvement
		* Albatross.WebClient 
			- Improved built in logging
			- Add http redirection support.
			- Add default polly retry capability
			- Add gzip encoding support
	* Albatross.Hosting.Utility
		- Improved console output capability
			- create utility to write out in tabular format and list format
		- clean Albatross.Hosting.Utility.BaseOption class so that its verb doesn't use single character.
			- verbose flag will trigger 
	* Albatross.Repository
		- add datelevel data support
* v4.1.0
	* upgrade to efcore 7
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