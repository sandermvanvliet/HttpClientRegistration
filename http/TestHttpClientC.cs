using System.Net.Http;
using Serilog;

namespace http
{
    public class TestHttpClientC : ITestClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public TestHttpClientC(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
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