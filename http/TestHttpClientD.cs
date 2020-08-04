using System.Net.Http;
using Serilog;

namespace http
{
    public class TestHttpClientD : ITestClient
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger _logger;

        public TestHttpClientD(IHttpClientFactory factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public void Execute()
        {
            var httpClient = _factory.CreateClient(nameof(TestHttpClientD));

            _logger.Information("{Name} => {HashCode}", this.GetType().Name, httpClient.GetHashCode());

            httpClient.DumpHandlers(_logger);

            httpClient
                .GetAsync("/")
                .GetAwaiter()
                .GetResult();
        }
    }
}