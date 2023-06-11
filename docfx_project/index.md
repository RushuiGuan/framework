### Albatross Framework
Albatross is a development framework that allow developers to quickly bootstrap an application with cross cutting functionaties such as logging, caching and config management.  It builds intergration points to other frameworks so that developers can work with a consistant platform without dealing with low level technical issues.

#### Here is a list of technogies adopted by Albatross

|Area|Technology|Assembly|
|-|-|-|
|CRM|EF Core|Albatross.Repository|
|Logging|Serilog|Albatross.Logging|
|Messaging|ZeroMQ\NetMQ|Albatross.Messaging|
|Serialization|System.Text.Json|
|Http Client|System.Net.Http.HttpClient, Polly|Albatross.WebClient|
|Code generation|custom|Albatross.CodeGen|
|Caching|MemCache, Redis, Polly|Albatross.Caching|
|Command line parsing|CommandLineParser|Albatross.Hosting.Utility|

#### Albatross has pre-bootstrapped hosting assemblies used as entry point for applications.  It also contains other support assemblies that covers other use cases.

|Assembly|Description|Target Framework|
|-|-|-|
|Albatross.Hosting|Bootstrapping assembly for creating a webapi and\or service application|Net7.0|
|Albatross.Hosting.Utility|Bootstrapping assembly for creating a command line console program|Net7.0|
|Albatross.Hosting.Test|Bootstrapping assembly for createing a unit test project|Net7.0|
|Albatross.Authentication|Utility for user authentication|Net6.0|
|Albatross.Caching|Utility for caching|NetStandard2.1|
|Albatross.Caching.Redis|Redis caching implementation|NetStandard2.1|
|Albatross.Caching.MemCache|MemCach implementation|NetStandard2.1|
|Albatross.CodeGen|CodeGen utility class|NetStandard2.1|
|Albatross.CodeGen.WebClient|CodeGen Implementation for WebApi Proxy|Net7.0|
|Albatross.Config|Utility for configuration management|NetStandard2.1|
|Albatross.Logging|Utility for logging|NetStandard2.1|
|Albatross.Repository|ORM setup using EFCore|Net7.0, EFCore7.0|
|Albatross.Repository.SqlServer|ORM Sql server setup|Net7.0, EFCore7.0|
|Albatross.Repository.PostgreSQL|ORM PostgreSQL server setup|Net7.0, EFCore7.0|
|Albatross|Utility assembly for shared code based that is not big enough to have its own assembly|NetStandard2.1|
|Albatross.WebClient|Utility for creation of http rest client|NetStandard2.1|
|Albatross.Messaging|Assembly for durable stream processing system|NetStandard2.1|