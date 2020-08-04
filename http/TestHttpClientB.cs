using System.Net.Http;
using Serilog;

namespace http
{
    public class TestHttpClientB : ITestClient
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public TestHttpClientB(IHttpClientFactory factory, ILogger logger)
        {
            _logger = logger;
            _httpClient = factory.CreateClient(nameof(TestHttpClientB));
        }

        public void Execute()
        {
            _logger.Information("{Name} => {HashCode}", this.GetType().Name, _httpClient.GetHashCode());

            _httpClient.DumpHandlers(_logger);

            _httpClient
                .GetAsync("/")
                .GetAwaiter()
                .GetResult();
        }
    }
}