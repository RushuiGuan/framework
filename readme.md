v4.1.0
	* change ImmutableEntity and MutableEntity to record class
		- Remove the Id column of both classes because we might not use an int id as the key
		- Remove the newly created constructor for both classes and revert to default constructor instead
			* the new constructor restricted the design of the derive class and makes everything a little harder
			* With the introduction of temporal class, it seems less necessary
	* create a temporal class to allow the full audit of a table
	* remove the Repository class
	* remove the following classes because they were never used
		- ReadOnlyDbSession
		- EFCoreTransaction
		- IRepository
		- ITransaction
