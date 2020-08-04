# HttpClient registrations

This repo contains a bunch of code to figure out how registering HttpClient really works and how IHttpClientFactory fits into that.

If you look in the `http` folder you'll find a console app that registers a basic client class `TestHttpClientA` (and B, C and D) that make a simple HTTP GET call.
They are registered in four different ways and when their `Execute()` method is called dump some info about the hash codes of the `HttpClient` instance and the handlers that this particular `HttpClient` has and _their_ hash codes.

This should allow us to see what the difference is between the ways of registering a `HttpClient` and what you get at runtime.

To test this the app will simply resolve each type four times in a loop and make a call.

Output is like:

```
[08:40:44 INF] TestHttpClientA => 37251161
[08:40:44 INF] Inner Handler Microsoft.Extensions.Http.LifetimeTrackingHttpMessageHandler => 66824994
[08:40:44 INF] Inner Handler Microsoft.Extensions.Http.Logging.LoggingScopeHttpMessageHandler => 64554036
[08:40:44 INF] Inner Handler http.TestHandlerA => 44115416
[08:40:44 INF] Inner Handler Microsoft.Extensions.Http.Logging.LoggingHttpMessageHandler => 61494432
[08:40:44 INF] Handler System.Net.Http.HttpClientHandler => 16578980
```

This'll be repeated 4 times for each type, the interesting bits are the first and last line. `HttpClientHandler` is the inner most handler and, according to docs, the one that is managed by the `IHttpClientFactory`. The first line is the typed client that either consumes `HttpClient` of the factory (look at the different types to see which).

Sample of a full run can be found [here](output.txt)

The short of it: _It doesn't matter how you register._

However it does matter how you resolve, if you resolve a typed client and keep that in a singleton you'll keep using the same `HttpClient` and `HttpClientHandler`.
