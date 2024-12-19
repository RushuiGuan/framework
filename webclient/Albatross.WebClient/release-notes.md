# 8.0.0
* Adding `CancellationToken` support
* Making `ClientBase` class thread-safe
* Remove redirect logic since `HttpClient` can now handle it itself
# 7.5.9
* Fix a string bug in [ClientBase](./ClientBase.cs) class where index out of bound exception would be thrown when the relative path is empty.
