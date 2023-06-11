### Albatross Framework
Albatross is a development framework that allow developers to quickly bootstrap an application with cross cutting functionaties such as logging, caching and config management.  It builds intergration point to other frameworks so that developers can work with a consistant platform without dealing with low level integration technical issues.

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

#### Albatross has pre-bootstrapped hosting assemblies.  It allows developers to create applications quickly.
* Albatross.Hosting - used for creating a webapi or service application.
* Albatross.Hosting.Utility - used for creating a command line console program.
* Albatross.Hosting.Test - used for creating unit test projects

