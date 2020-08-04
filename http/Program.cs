using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace http
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            serviceCollection.AddSingleton<ILogger>(logger);

            serviceCollection.AddTransient<TestHandlerA>();
            serviceCollection.AddTransient<TestHandlerB>();
            serviceCollection.AddTransient<TestHandlerC>();
            serviceCollection.AddTransient<TestHandlerD>();

            // Register like we do it now
            serviceCollection.AddHttpClient<TestHttpClientA>()
                .AddHttpMessageHandler<TestHandlerA>();

            // Register a named client
            serviceCollection
                .AddHttpClient(nameof(TestHttpClientB), client =>
                {
                    client.BaseAddress = new Uri("http://motherfuckingwebsite.com/");
                })
                .AddHttpMessageHandler<TestHandlerB>();
            // Because for B we're using a named client, register the type here
            serviceCollection.AddSingleton<TestHttpClientB>();

            // Register a typed client with configuration
            serviceCollection.AddHttpClient<TestHttpClientC>(nameof(TestHttpClientC), client =>
                {
                    client.BaseAddress = new Uri("https://securemotherfuckingwebsite.com/");
                })
                .AddHttpMessageHandler<TestHandlerC>();

            // Register a named client
            serviceCollection
                .AddHttpClient(nameof(TestHttpClientD), client =>
                {
                    client.BaseAddress = new Uri("http://motherfuckingwebsite.com/");
                })
                .AddHttpMessageHandler<TestHandlerD>();
            // Because for B we're using a named client, register the type here
            serviceCollection.AddSingleton<TestHttpClientD>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // A straight up consumes a HttpClient
            RunWithType<TestHttpClientA>(serviceProvider);
            // B consumes a IHttpClientFactory and creates the HttpClient in the constructor
            RunWithType<TestHttpClientB>(serviceProvider);
            // C consumes a HttpClient directly but is registered both with a named client and typed
            RunWithType<TestHttpClientC>(serviceProvider);
            // D consumes a IHttpClientFactory and creates the HttpClient in the method call
            RunWithType<TestHttpClientD>(serviceProvider);
        }

        private static void RunWithType<TClient>(IServiceProvider serviceProvider) where TClient : ITestClient
        {
            Console.WriteLine($"\n\nStarting run with {typeof(TClient).Name}\n");

            for (var j = 0; j < 4; j++)
            {
                var httpClient = serviceProvider.GetService<TClient>();
                httpClient.Execute();
            }
        }
    }
}
