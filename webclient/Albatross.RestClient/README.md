# Albatross.WebClient
Extends the functionalities of System.Net.Http.HttpClient class

## Features
* Http request\response raw content logging
* Custom json serialization configuration provided by [Albatross.Serialization](../../serialization/Albatross.Serialization/)
* Handles GZip Content type
* Handles Redirect (redirect is now part of core functionality of the HttpClient class)
* WebClient code generation provided by [Albatross.CodeGen.CommandLine](../../codegen/Albatross.CodeGen.CommandLine/)
* Retry Policy provided by [Polly v7](https://github.com/App-vNext/Polly)