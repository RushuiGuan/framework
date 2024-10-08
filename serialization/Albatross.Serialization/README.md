# Albatross.Serialization
Provides additional functionalities for serialization using System.Text.Json.

# Features
* [IJsonSettings](./IJsonSettings.cs) interface to standardalize JsonSetting creation
	
	It is not well documented but construction of `JsonSerializerOptions` is an expensive process.  The options are meant to be reused and shared.
	
