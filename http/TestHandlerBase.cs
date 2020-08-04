using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace http
{
    public abstract class TestHandlerBase : DelegatingHandler
    {
        private readonly ILogger _logger;

        protected TestHandlerBase(ILogger logger)
        {
            _logger = logger;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.Information("Handler {Name} => {Hashcode}", GetType().Name, GetHashCode());

            var servicePoint = ServicePointManager.FindServicePoint(request.RequestUri);

            _logger.Information("ServicePoint {Name}, CurrentConnections: {Count}, Address {Address}", servicePoint.ConnectionName, servicePoint.CurrentConnections, servicePoint.Address);

            return base.SendAsync(request, cancellationToken);
        }
    }
}